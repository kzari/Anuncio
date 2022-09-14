using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Services;
using Lopes.Domain.Commons;
using MediatR;
using System.Collections.Concurrent;
using Lopes.Anuncio.Domain.Entidades;
using Lopes.Anuncio.Domain.Models.Imovel;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Domain.Handlers
{
    public class AnuncioAtualizadorHandler : IRequestHandler<AnunciosAtualizacaoCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPortalAtualizadorFactory _portalAtualizadorFactory;
        private readonly List<DadosImovel> _dadosImoveisCache;

        public AnuncioAtualizadorHandler(ILogger logger,
                                         IServiceProvider serviceProvider,
                                         IPortalAtualizadorFactory portalAtualizadorFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _portalAtualizadorFactory = portalAtualizadorFactory;
            _dadosImoveisCache = new List<DadosImovel>();
        }


        public Task<bool> Handle(AnunciosAtualizacaoCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<AnuncioCota> anuncios = request.Anuncios;
            ILogger logger = request.Logger ?? _logger;

            int totalAnuncios = anuncios.Count();

            var anunciosAgrupados = anuncios.GroupBy(_ => new { _.Portal, _.IdEmpresa }, (key, group) => new
            {
                Portal = key.Portal,
                IdEmpresa = key.IdEmpresa,
                Anuncios = group.ToList()
            }).ToList();
            int qtdeCotas = anunciosAgrupados.Count;

            logger.Info($"{totalAnuncios} anúncios encontrados para atualização");

            IProgresso progressoGeral = logger.ObterProgresso(anunciosAgrupados.Count, 95, $">> Atualização de imóveis nos portais.");
            progressoGeral.Atualizar($"Processando {qtdeCotas} cotas.");

            var partitioner = Partitioner.Create(anunciosAgrupados);
            var partitions = partitioner.GetPartitions(Environment.ProcessorCount);
            //var partitions = partitioner.GetPartitions(1);

            int partitionIds = 0;
            int cotaAtual = 0;

            Task[] tasks = partitions.Select(partition => Task.Run(() =>
            {
                int partitionId = partitionIds++;

                IStatusAnuncioService statusAnuncioService = (IStatusAnuncioService)_serviceProvider.GetService(typeof(IStatusAnuncioService));
                IAnuncioStatusRepositorioGravacao imovelAtualizacaoPortaisRepository = (IAnuncioStatusRepositorioGravacao)_serviceProvider.GetService(typeof(IAnuncioStatusRepositorioGravacao));

                using (partition)
                {
                    while (partition.MoveNext())
                    {
                        cotaAtual++;
                        var portalEmpresa = partition.Current;

                        int idEmpresa = portalEmpresa.IdEmpresa;
                        Portal portal = portalEmpresa.Portal;
                        List<AnuncioCota> anuncios = portalEmpresa.Anuncios;
                        int qtdeAnuncios = anuncios.Count;
                        List<int> imoveisParaRemover = new();
                        List<int> imoveisParaAtualizar = new();
                        int jaRemovidos = 0;
                        int jaAtualizados = 0;

                        IProgresso progresso = logger.ObterProgresso(qtdeAnuncios, 95, textoInicial: $"P{partitionId.ToString().PadLeft(2)} E: {idEmpresa.ToString().PadRight(5)} P: {portal.ToString().PadRight(10)} Anúncios: {qtdeAnuncios.ToString().PadLeft(5)} ");

                        progresso.Mensagem("1. Verificando o status...", percentualConcluido: 10);

                        IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador(portal, idEmpresa);

                        int[] idImoveisNoPortal = atualizador.ObterIdImoveisNoPortal().ToArray();

                        foreach (AnuncioCota anuncio in anuncios)
                        {
                            bool imovelNoPortal = idImoveisNoPortal.Contains(anuncio.IdImovel);

                            switch (statusAnuncioService.VerificarStatusImovelPortal(anuncio, imovelNoPortal))
                            {
                                case StatusAnuncioPortal.Atualizado:
                                    jaAtualizados++;
                                    continue;

                                case StatusAnuncioPortal.Removido:
                                    jaRemovidos++;
                                    continue;

                                case StatusAnuncioPortal.ARemover:
                                    imoveisParaRemover.Add(anuncio.IdImovel);
                                    break;

                                case StatusAnuncioPortal.Desatualizado:
                                    imoveisParaAtualizar.Add(anuncio.IdImovel);
                                    break;
                            }
                        }

                        progresso.Mensagem($"2. Removendo {imoveisParaRemover.Count} anúncios...", percentualConcluido: 10);
                        atualizador.RemoverImoveis(imoveisParaRemover.ToArray(), progresso);
                        List<AnuncioAtualizacao> atualizacoes = imoveisParaRemover.Select(_ => new AnuncioAtualizacao(portal, _, idEmpresa, AtualizacaoAcao.Exclusao)).ToList();

                        progresso.Mensagem($"3. Atualizando/Adicionando {imoveisParaAtualizar.Count} anúncios...", percentualConcluido: 50);
                        atualizacoes.AddRange(AtualizarAdicionarAnuncios(anuncios, imoveisParaAtualizar, idEmpresa, portal, atualizador, progresso));

                        progresso.Mensagem($"4. Registrando o status...", percentualConcluido: 80);
                        RegistrarAtualizacao(atualizacoes, cancellationToken);

                        progresso.Mensagem($"Concluído. R: {imoveisParaRemover.Count.ToString().PadLeft(5)} / {jaRemovidos.ToString().PadLeft(5)} A: {imoveisParaAtualizar.Count.ToString().PadLeft(5)} / {jaAtualizados.ToString().PadLeft(5)}).", percentualConcluido: 100);

                        progressoGeral.Atualizar($"Processando cotas {cotaAtual} de {qtdeCotas}.");
                    }
                }
            })).ToArray();
            Task.WaitAll(tasks, cancellationToken);

            progressoGeral.Mensagem($"Atualização concluída. {qtdeCotas} cotas, {totalAnuncios} anúncios.", percentualConcluido: 100);


            return Task.FromResult(true);
        }

        private bool RegistrarAtualizacao(List<AnuncioAtualizacao> atualizacoes, CancellationToken cancellationToken)
        {
            var registrarAtualizacaoHandler = _serviceProvider.ObterServico<IRequestHandler<RegistroAtualizacoesCommand, bool>>();
            return registrarAtualizacaoHandler.Handle(new RegistroAtualizacoesCommand(atualizacoes), cancellationToken).Result;
        }

        private List<AnuncioAtualizacao> AtualizarAdicionarAnuncios(IEnumerable<AnuncioCota> anuncios,
                                                                               IEnumerable<int> imoveisParaAtualizar,
                                                                               int idEmpresa,
                                                                               Portal portal,
                                                                               IPortalAtualizador atualizador,
                                                                               IProgresso progresso)
        {
            if (!imoveisParaAtualizar.Any())
                return new List<AnuncioAtualizacao>();

            progresso.Mensagem($"3. Obtendo dados dos {imoveisParaAtualizar.Count()} imóveis.", percentualConcluido: 20);

            

            IEnumerable<DadosImovel> imoveis = ObterDadosImoveisCache(imoveisParaAtualizar.ToArray(), progresso);

            //TODO: tratar imóveis não encontrados

            List<AnuncioAtualizacao> atualizacoes = new();

            if (!imoveis.Any())
                return atualizacoes;

            foreach (DadosImovel imovel in imoveis)
            {
                imovel.CodigoClientePortal = anuncios.FirstOrDefault(_ => _.IdImovel == imovel.Dados.IdImovel).CodigoClientePortal;
                atualizacoes.Add(new AnuncioAtualizacao(portal, imovel.Dados.IdImovel, idEmpresa, AtualizacaoAcao.Atualizacao));
            }

            progresso.Mensagem($"3. Atualizando imóveis...", percentualConcluido: 30);
            atualizador.InserirAtualizarImoveis(imoveis, progresso: progresso);

            return atualizacoes;
        }


        private IEnumerable<DadosImovel> ObterDadosImoveisCache(int[] idImoveis, IProgresso progresso)
        {
            IEnumerable<DadosImovel> imoveisCacheados;
            lock (_dadosImoveisCache)
            {
                imoveisCacheados = _dadosImoveisCache.ToList().Where(_ => idImoveis.Contains(_.Dados.IdImovel)) ?? new List<DadosImovel>();
            }

            int[] idImoveisNaoCacheados = idImoveis.Where(_ => !imoveisCacheados.Select(_ => _.Dados.IdImovel).Contains(_)).ToArray() ?? idImoveis;
            if (idImoveisNaoCacheados.Any())
            {
                IEnumerable<DadosImovel> imoveisNaoCacheados = ObterDadosImovel(idImoveisNaoCacheados, progresso);
                if (imoveisNaoCacheados.Any())
                {
                    lock (_dadosImoveisCache)
                        _dadosImoveisCache.AddRange(imoveisNaoCacheados);
                    return imoveisCacheados.Concat(imoveisNaoCacheados);
                }
            }

            return imoveisCacheados;
        }

        public IEnumerable<DadosImovel> ObterDadosImovel(int[] idImoveis, IProgresso progresso = null)
        {
            if (progresso != null)
                progresso.Atualizar($"Obtendo dados principais de {idImoveis.Length} imóveis.");

            IImovelRepository imovelRepository = (IImovelRepository)_serviceProvider.GetService(typeof(IImovelRepository));

            List<DadosPrincipais> dadosPrincipais = imovelRepository.ObterDadosImoveis(idImoveis).ToList();

            int[] idImoveisResgatados = dadosPrincipais.Select(_ => _.IdImovel).ToArray();

            if (progresso != null)
                progresso.Atualizar($"Obtendo caracteristicas de {idImoveisResgatados.Length} imóveis.");

            List<Caracteristica> caracteristicas = imovelRepository.ObterCaracteristicas(idImoveisResgatados).ToList();

            if (progresso != null)
                progresso.Atualizar($"Obtendo Tour virtual e Vídeos de {idImoveisResgatados.Count()} imóveis.");

            IDictionary<int, string[]> urlTours = imovelRepository.ObterUrlTourVirtuais(idImoveisResgatados);
            IDictionary<int, string[]> urlVideos = imovelRepository.ObterUrlVideos(idImoveisResgatados);
            IEnumerable<Fotos> fotos = imovelRepository.ObterFotos(idImoveisResgatados);

            if (progresso != null)
                progresso.Atualizar($"Preenchendo informações de {idImoveisResgatados.Length} imóveis.");

            List<DadosImovel> imoveis = new List<DadosImovel>();
            foreach (DadosPrincipais dados in dadosPrincipais)
            {
                DadosImovel imovel = new(dados);

                imovel.Caracteristicas = caracteristicas.Where(_ => _.IdImovel == dados.IdImovel);
                imovel.UrlTourVirtuais = urlTours.Where(_ => _.Key == dados.IdImovel).SelectMany(_ => _.Value);
                imovel.UrlVideos = urlVideos.Where(_ => _.Key == dados.IdImovel).SelectMany(_ => _.Value);
                imovel.Imagens = fotos.Where(_ => _.IdImovel == dados.IdImovel);

                yield return imovel;
            }
        }
    }
}
