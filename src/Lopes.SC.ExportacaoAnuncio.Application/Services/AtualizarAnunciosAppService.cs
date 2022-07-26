using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;
using System.Collections.Concurrent;


namespace Lopes.SC.ExportacaoAnuncio.Application.Services
{
    public class AtualizarAnunciosAppService : IAtualizarAnunciosAppService
    {
        private readonly ILogger _logger;
        private readonly IAnuncioAppService _anuncioAppService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPortalAtualizadorFactory _portalAtualizadorFactory;

        private readonly List<DadosImovel> _dadosImoveisCache;

        public AtualizarAnunciosAppService(ILogger logger,
                                              IAnuncioAppService anuncioAppService,
                                              IServiceProvider serviceProvider,
                                              IPortalAtualizadorFactory portalAtualizadorFactory)
        {
            _logger = logger;
            _anuncioAppService = anuncioAppService;
            _dadosImoveisCache = new List<DadosImovel>();
            _serviceProvider = serviceProvider;
            _portalAtualizadorFactory = portalAtualizadorFactory;
        }



        public void AtualizarPorImoveis(int[] idImoveis)
        {
            IEnumerable<Anuncio> anuncios = _anuncioAppService.ObterAnunciosPorImoveis(idImoveis).OrderBy(_ => _.IdImovel)
                                                                                                 .ToList();
            AtualizarImoveisXMLs(anuncios);
        }

        public void AtualizarPorCotas(int[] idCotas)
        {
            IEnumerable<Anuncio> anuncios = _anuncioAppService.ObterAnunciosPorCotas(idCotas).OrderBy(_ => _.IdImovel)
                                                                                             .ToList();
            AtualizarImoveisXMLs(anuncios);
        }

        public void AtualizarPorPortais(Portal[] portais)
        {
            IEnumerable<Anuncio> anuncios = _anuncioAppService.ObterAnunciosPorPortais(portais).OrderBy(_ => _.Portal)
                                                                                               .ThenBy(_ => _.IdEmpresa)
                                                                                               .ThenBy(_ => _.IdImovel)
                                                                                               .ToList();
            AtualizarImoveisXMLs(anuncios);
        }

        private void AtualizarImoveisXMLs(IEnumerable<Anuncio> anuncios)
        {
            int contPortalEmpresa = 0;
            int totalAnuncios = anuncios.Count();
            int partitionIds = 0;

            var anunciosAgrupados = anuncios.GroupBy(_ => new { _.Portal, _.IdEmpresa }, (key, group) => new
            {
                Portal = key.Portal,
                IdEmpresa = key.IdEmpresa,
                Anuncios = group.ToList()
            }).ToList();

            IProgresso progressoGeral = _logger.ObterProgresso(anunciosAgrupados.Count(), 100);
            progressoGeral.Atualizar(contPortalEmpresa, $"Progresso geral 0 de {anunciosAgrupados.Count()}.");

            var partitioner = Partitioner.Create(anunciosAgrupados);
            var partitions = partitioner.GetPartitions(Environment.ProcessorCount);
            //var partitions = partitioner.GetPartitions(2);

            Task[] tasks = partitions.Select(partition => Task.Run(() =>
            {
                int partitionId = partitionIds++;

                IStatusAnuncioService statusAnuncioService = (IStatusAnuncioService)_serviceProvider.GetService(typeof(IStatusAnuncioService));
                IImovelAtualizacaoPortaisRepository imovelAtualizacaoPortaisRepository = (IImovelAtualizacaoPortaisRepository)_serviceProvider.GetService(typeof(IImovelAtualizacaoPortaisRepository));
                IDadosImovelAppService dadosImovelAppService = (IDadosImovelAppService)_serviceProvider.GetService(typeof(IDadosImovelAppService));


                using (partition)
                    while (partition.MoveNext())
                    {
                        var portalEmpresa = partition.Current;
                        int i = 0;
                        int removidos = 0;
                        int jaRemovidos = 0;
                        int jaAtualizados  = 0;

                        IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador(portalEmpresa.Portal, portalEmpresa.IdEmpresa);

                        IProgresso progresso = _logger.ObterProgresso(portalEmpresa.Anuncios.Count(), 100);

                        List<DadosImovel> imoveisParaAtualizar = new List<DadosImovel>();

                        foreach (Anuncio anuncio in portalEmpresa.Anuncios)
                        {
                            i++;

                            //TODO: Obter todos os imóveis no portal de uma vez só (performance)
                            bool imovelNoPortal = atualizador.ImovelNoPortal(anuncio.IdImovel);

                            StatusAnuncioPortal statusImovelPortal = statusAnuncioService.VerificarStatusImovelPortal(anuncio, imovelNoPortal);
                            switch (statusImovelPortal)
                            {
                                case StatusAnuncioPortal.Atualizado:
                                    jaAtualizados++;
                                    continue;

                                case StatusAnuncioPortal.Removido:
                                    jaRemovidos++;
                                    continue;

                                case StatusAnuncioPortal.ARemover:
                                    atualizador.RemoverImovel(anuncio.IdImovel);

                                    imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(new AnuncioAtualizacao(anuncio.Portal,
                                                                                                                   anuncio.IdImovel,
                                                                                                                   anuncio.IdEmpresa,
                                                                                                                   AtualizacaoAcao.Exclusao));
                                    removidos++;
                                    break;

                                case StatusAnuncioPortal.Desatualizado:
                                    {
                                        DadosImovel dados = ObterDadosImovel(anuncio.IdImovel, dadosImovelAppService);
                                        if (dados == null)
                                        {
                                            _logger.Error($"Imóvel não encontrado {anuncio.IdImovel}.");
                                        }
                                        else
                                        {
                                            dados.CodigoClientePortal = anuncio.CodigoClientePortal;

                                            imoveisParaAtualizar.Add(dados);
                                        }
                                        break;
                                    }
                            }

                            MostrarProgresso(progresso, partitionId, portalEmpresa.Portal, portalEmpresa.IdEmpresa, portalEmpresa.Anuncios.Count(), i);
                        }

                        Atualizar(imoveisParaAtualizar, portalEmpresa.IdEmpresa, portalEmpresa.Portal, atualizador, imovelAtualizacaoPortaisRepository);

                        progressoGeral.Atualizar(++contPortalEmpresa, $"Progresso geral {contPortalEmpresa} de {anunciosAgrupados.Count()}.");
                    }
            })).ToArray();
            Task.WaitAll(tasks);
        }

        private static void MostrarProgresso(IProgresso progresso, int partitionId, Portal portal, int idEmpresa, int qtdeAnuncios, int qtdeAtual)
        {
            if (qtdeAtual < (qtdeAnuncios - 5) && qtdeAnuncios % 5 > 0) //Mostrando de 5 em 5
                return;

            string mensagem = $"P{partitionId} - Portal: {portal.ToString().PadRight(10)}" +
                            $" Empresa: {idEmpresa.ToString().PadLeft(3)}" +
                            $" {qtdeAtual.ToString().PadLeft(5)} de {qtdeAnuncios.ToString().PadLeft(5)} anúncios.";

            progresso.Atualizar(qtdeAtual, mensagem);
        }
        //private void MostrarProgresso(int partitionId, int i)
        //{
        //    lock (progresso)
        //    {
        //        if (!progresso.TryGetValue(partitionId, out int valor))
        //            progresso.Add(partitionId, i);
        //        else
        //            progresso[partitionId] = i;

        //        string backspaces = string.Empty;
        //        string todos = string.Empty;

        //        foreach (var item in progresso.ToList().OrderBy(_ => _.Key))
        //        {
        //            backspaces += "\b\b\b\b\b\b\b\b\b\b\b ";
        //            string template = $"  P{item.Key} {item.Value.ToString().PadLeft(5)}";
        //            todos += template;
        //        }

        //        todos = backspaces + todos;
        //        Console.Write($"{todos} -- ");
        //    }
        //}

        private static void Atualizar(List<DadosImovel> imoveisParaAtualizar, 
                               int idEmpresa, 
                               Portal portal, 
                               IPortalAtualizador atualizador,
                               IImovelAtualizacaoPortaisRepository imovelAtualizacaoPortaisRepository)
        {
            if (!imoveisParaAtualizar.Any())
                return;

            atualizador.InserirAtualizarImoveis(imoveisParaAtualizar);

            foreach (int idImovel in imoveisParaAtualizar.Select(_ => _.Dados.IdImovel))
            {
                imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(new AnuncioAtualizacao(portal, idImovel, idEmpresa, AtualizacaoAcao.Atualizacao));
            }
        }

        private DadosImovel? ObterDadosImovel(int idImovel, IDadosImovelAppService dadosImovelAppService)
        {
            DadosImovel? dados;
            lock (_dadosImoveisCache)
            { 
                dados = _dadosImoveisCache.ToList().FirstOrDefault(_ => _.Dados.IdImovel == idImovel);
                if (dados != null)
                    return dados;
            }

            dados = dadosImovelAppService.ObterDadosImovel(idImovel);
            if (dados == null)
                return null;

            lock (_dadosImoveisCache)
                _dadosImoveisCache.Add(dados);

            return dados;
        }
    }
}