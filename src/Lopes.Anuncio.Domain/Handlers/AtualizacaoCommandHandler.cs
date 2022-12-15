using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Services;
using Lopes.Domain.Commons;
using MediatR;
using System.Collections.Concurrent;
using Lopes.Anuncio.Domain.Entidades;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Domain.Commons.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.Anuncio.Domain.Handlers
{
    /// <summary>
    /// Faz a atualização dos anúncios nos portais
    /// </summary>
    public class AtualizacaoCommandHandler : IRequestHandler<AnunciosAtualizacaoCommand, bool>
    {
        
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPortalAtualizadorFactory _portalAtualizadorFactory;
        private readonly ICacheService _cacheService;

        public AtualizacaoCommandHandler(ILogger logger,
                                         IServiceProvider serviceProvider,
                                         IPortalAtualizadorFactory portalAtualizadorFactory,
                                         ICacheService cacheService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _portalAtualizadorFactory = portalAtualizadorFactory;
            _cacheService = cacheService;
        }


        public Task<bool> Handle(AnunciosAtualizacaoCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<AnuncioCota> anuncios = request.Anuncios;
            ILogger logger = request.Logger ?? _logger;

            int totalAnuncios = anuncios.Count();

            var anunciosAgrupados = anuncios.GroupBy(_ => new { _.Portal, _.IdFranquia, _.IdCota }, (key, group) => new
            {
                Portal = key.Portal,
                IdFranquia = key.IdFranquia,
                IdCota = key.IdCota,
                Anuncios = group.ToList()
            }).ToList();
            int qtdeCotas = anunciosAgrupados.Count;

            logger.Info($"{totalAnuncios} anúncios encontrados para atualização. {qtdeCotas} cota(s).");

            IProgresso progressoGeral = logger.ObterProgresso(anunciosAgrupados.Count, 95, $">> Atualização de imóveis nos portais.");
            progressoGeral.NovaMensagem($"Processando {qtdeCotas} cota(s).");

            var partitioner = Partitioner.Create(anunciosAgrupados);
            var partitions = partitioner.GetPartitions(Environment.ProcessorCount);
            //var partitions = partitioner.GetPartitions(2);

            int partitionIds = 0;
            int cotaAtual = 0;

            Task[] tasks = partitions.Select(partition => Task.Run(() =>
            {
                int partitionId = partitionIds++;

                IStatusAnuncioService statusAnuncioService = _serviceProvider.ObterServico<IStatusAnuncioService>();
                IAnuncioStatusRepositorio imovelAtualizacaoPortaisDadosService = _serviceProvider.ObterServico<IAnuncioStatusRepositorio>();

                using (partition)
                {
                    while (partition.MoveNext())
                    {
                        cotaAtual++;
                        var portalEmpresa = partition.Current;

                        int idFranquia = portalEmpresa.IdFranquia;
                        string nomeFranquia = portalEmpresa.Anuncios.First().NomeFranquia;
                        int idCota = portalEmpresa.Anuncios.First().IdCota;
                        Portal portal = portalEmpresa.Portal;
                        List<AnuncioStatus> anuncios = portalEmpresa.Anuncios.Select(_ => new AnuncioStatus(_)).ToList();
                        int qtdeAnuncios = anuncios.Count;

                        IProgresso progresso = logger.ObterProgresso(qtdeAnuncios, 95, textoInicial: $"P{partitionId.ToString().PadLeft(2)} E: {idFranquia.ToString().PadRight(5)} P: {portal.ToString().PadRight(10)} Anúncios: {qtdeAnuncios.ToString().PadLeft(5)} ");

                        progresso.Mensagem($"Franquia: {nomeFranquia}  | Id Cota: {idCota}", percentualConcluido: 5);
                        progresso.Mensagem("1. Verificando o status...", percentualConcluido: 10);

                        IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador(portal, idFranquia);

                        DefinirStatusAnuncio(statusAnuncioService, anuncios, atualizador);

                        List<AnuncioAtualizacao> atualizacoes = new();

                        IEnumerable<AnuncioAtualizacao> anunciosRemover = RemoverAnuncios(anuncios, progresso, atualizador);
                        atualizacoes.AddRange(anunciosRemover);

                        IEnumerable<AnuncioAtualizacao> anunciosAtualizar = AtualizarAdicionarAnuncios(anuncios, idFranquia, portal, atualizador, progresso);
                        atualizacoes.AddRange(anunciosAtualizar);

                        RegistrarAtualizacao(atualizacoes, progresso, cancellationToken);

                        progresso.Mensagem($"Concluído. Removidos: {anunciosRemover.Count().ToString().PadLeft(5)} Atualizados/Inseridos: {anunciosAtualizar.Count().ToString().PadLeft(5)}.", percentualConcluido: 100);

                        progressoGeral.NovaMensagem($"Processando cotas {cotaAtual} de {qtdeCotas}.");
                    }
                }
            })).ToArray();
            Task.WaitAll(tasks, cancellationToken);

            progressoGeral.Mensagem($"Atualização concluída. {qtdeCotas} cotas, {totalAnuncios} anúncios.", percentualConcluido: 100);


            return Task.FromResult(true);
        }


        private static void DefinirStatusAnuncio(IStatusAnuncioService statusAnuncioService, List<AnuncioStatus> anuncios, IPortalAtualizador atualizador)
        {
            int[] idProdutosNoPortal = atualizador.ObterIdProdutosNoPortal().ToArray();

            foreach (AnuncioStatus anuncio in anuncios)
            {
                bool imovelNoPortal = idProdutosNoPortal.Contains(anuncio.AnuncioCota.IdProduto);

                anuncio.Status = statusAnuncioService.VerificarStatusProdutoPortal(anuncio.AnuncioCota, imovelNoPortal);
            }
        }

        private static IEnumerable<AnuncioAtualizacao> RemoverAnuncios(IEnumerable<AnuncioStatus> anuncios, IProgresso progresso, IPortalAtualizador atualizador)
        {
            int[] idProdutosRemover = anuncios.Where(_ => _.Status == StatusAnuncioPortal.ARemover).Select(_ => _.AnuncioCota.IdProduto).ToArray();
            if (idProdutosRemover.Length == 0)
                return Enumerable.Empty<AnuncioAtualizacao>();

            progresso.Mensagem($"2. Removendo {idProdutosRemover.Count()} anúncios...", percentualConcluido: 10);

            atualizador.RemoverProdutos(idProdutosRemover, progresso);

            int idFranquia = anuncios.FirstOrDefault().AnuncioCota.IdFranquia;
            Portal portal = anuncios.FirstOrDefault().AnuncioCota.Portal;
            return idProdutosRemover.Select(_ => new AnuncioAtualizacao(portal, _, idFranquia, AtualizacaoAcao.Exclusao)).ToList();
        }
        private List<AnuncioAtualizacao> AtualizarAdicionarAnuncios(IEnumerable<AnuncioStatus> anuncios,
                                                                    int idEmpresa,
                                                                    Portal portal,
                                                                    IPortalAtualizador atualizador,
                                                                    IProgresso progresso)
        {
            int[] produtosParaAtualizar = anuncios.Where(_ => _.Status == StatusAnuncioPortal.Desatualizado).Select(_ => _.AnuncioCota.IdProduto).ToArray();
            progresso.Mensagem($"3. Atualizando/Adicionando {produtosParaAtualizar.Count()} anúncios...", percentualConcluido: 50);

            if (!produtosParaAtualizar.Any())
                return new List<AnuncioAtualizacao>();

            progresso.Mensagem($"3. Obtendo dados dos {produtosParaAtualizar.Count()} produtos.", percentualConcluido: 20);

            IEnumerable<Produto> produtos = _serviceProvider.ObterServico<IProdutoService>().ObterDados(produtosParaAtualizar.ToArray(), progresso).ToList();

            List<AnuncioAtualizacao> atualizacoes = new();

            if (!produtos.Any())
                return atualizacoes;

            foreach (Produto imovel in produtos)
            {
                imovel.CodigoClientePortal = anuncios.FirstOrDefault(_ => _.AnuncioCota.IdProduto == imovel.Dados.IdProduto).AnuncioCota.CodigoClientePortal;
                atualizacoes.Add(new AnuncioAtualizacao(portal, imovel.Dados.IdProduto, idEmpresa, AtualizacaoAcao.Atualizacao));
            }

            progresso.Mensagem($"3. Atualizando produtos...", percentualConcluido: 30);
            atualizador.InserirAtualizarProdutos(produtos, progresso: progresso);

            return atualizacoes;
        }

        private bool RegistrarAtualizacao(List<AnuncioAtualizacao> atualizacoes, IProgresso progresso, CancellationToken cancellationToken)
        {
            progresso.Mensagem($"4. Registrando o status...", percentualConcluido: 80);

            using IServiceScope scope = _serviceProvider.CreateScope();
            {
                var handler = scope.ServiceProvider.ObterServico<IRequestHandler<RegistroAtualizacoesCommand, bool>>();

                Task<bool>? result = handler.Handle(new RegistroAtualizacoesCommand(atualizacoes, progresso), cancellationToken);

                return result.Result;
            }   
        }
    }
}
