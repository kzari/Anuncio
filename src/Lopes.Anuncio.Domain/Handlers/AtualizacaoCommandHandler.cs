using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Services;
using Lopes.Domain.Commons;
using MediatR;
using System.Collections.Concurrent;
using Lopes.Anuncio.Domain.Entidades;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Infra.Commons.Extensions;

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

        public AtualizacaoCommandHandler(ILogger logger,
                                         IServiceProvider serviceProvider,
                                         IPortalAtualizadorFactory portalAtualizadorFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _portalAtualizadorFactory = portalAtualizadorFactory;
        }


        public Task<bool> Handle(AnunciosAtualizacaoCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<AnuncioCota> anuncios = request.Anuncios.ToList();
            ILogger logger = request.Logger ?? _logger;

            List<AnunciosAgrupados> anunciosAgrupados = AgruparAnuncios(anuncios);
            int qtdeCotas = anunciosAgrupados.Count;

            logger.Info($"{anuncios.Count()} anúncios encontrados para atualização. {qtdeCotas} cota(s).");

            IProgresso progressoGeral = logger.NovoProgresso(anunciosAgrupados.Count, 95, ">> -- Progresso geral --");
            progressoGeral.NovaMensagem($">>> Processando {qtdeCotas} cota(s). -----------");

            IList<IEnumerator<AnunciosAgrupados>> partitions = ObterParticoes(anunciosAgrupados);
            //IList<IEnumerator<AnuncioAgrupado>> partitions = ObterParticoes(anunciosAgrupados, qtdeProcessadores: 2);

            int partitionIds = 0;
            int cotaAtual = 0;

            Task[] tasks = partitions.Select(partition =>
                Task.Run(() => ProcessarAgrupamento(partition, logger, qtdeCotas, progressoGeral, ref partitionIds, ref cotaAtual, cancellationToken))).ToArray();
            Task.WaitAll(tasks, cancellationToken);

            progressoGeral.Mensagem($"Atualização concluída. {qtdeCotas} cota(s), {anuncios.Count()} anúncio(s).", percentualConcluido: 100);

            return Task.FromResult(true);
        }


        private void ProcessarAgrupamento(IEnumerator<AnunciosAgrupados> partition,
                                          ILogger logger,
                                          int qtdeCotas,
                                          IProgresso progressoGeral,
                                          ref int partitionIds,
                                          ref int cotaAtual,
                                          CancellationToken cancellationToken)
        {
            int partitionId = partitionIds++;

            IStatusAnuncioService statusAnuncioService = _serviceProvider.ObterServico<IStatusAnuncioService>();

            using (partition)
            {
                while (partition.MoveNext())
                {
                    AnunciosAgrupados anunciosAgrupados = partition.Current;
                    int idFranquia = anunciosAgrupados.IdFranquia;
                    Portal portal = anunciosAgrupados.Portal;
                    List<AnuncioStatus> anuncios = anunciosAgrupados.Anuncios.Select(_ => new AnuncioStatus(_)).ToList();

                    IProgresso progresso = logger.NovoProgresso(anuncios.Count, 95, textoInicial: $"P{partitionId,2} F: {idFranquia,-5} - {portal,-10} Anun.(s): {anuncios.Count,5} ");

                    progresso.Mensagem("1. Verificando o status...", percentualConcluido: 10);
                    IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador(portal, idFranquia);
                    DefinirStatusAnuncio(statusAnuncioService, anuncios, atualizador);

                    List<AnuncioAtualizacao> atualizacoes = new();

                    IEnumerable<AnuncioAtualizacao> anunciosRemover = RemoverAnuncios(anuncios, progresso, atualizador);
                    atualizacoes.AddRange(anunciosRemover);

                    IEnumerable<AnuncioAtualizacao> anunciosAtualizar = AtualizarAdicionarAnuncios(anuncios, idFranquia, portal, atualizador, progresso);
                    atualizacoes.AddRange(anunciosAtualizar);

                    RegistrarAtualizacao(atualizacoes, cancellationToken, progresso);

                    progresso.Mensagem($"Concluído. Rem.: {anunciosRemover.Count(),5} A/I: {anunciosAtualizar.Count(),5}.", percentualConcluido: 100);

                    progressoGeral.NovaMensagem($"Processando cotas {++cotaAtual} de {qtdeCotas}.");
                }
            }
        }

        private static IList<IEnumerator<T>> ObterParticoes<T>(IEnumerable<T> anunciosAgrupados, int? qtdeProcessadores = null)
        {
            OrderablePartitioner<T> partitioner = Partitioner.Create(anunciosAgrupados);
            return partitioner.GetPartitions(qtdeProcessadores ?? Environment.ProcessorCount);
        }

        private static List<AnunciosAgrupados> AgruparAnuncios(IEnumerable<AnuncioCota> anuncios)
        {
            return anuncios.GroupBy(_ => new { _.Portal, _.IdFranquia, _.IdCota }, (key, group) => new AnunciosAgrupados
            {
                Portal = key.Portal,
                IdFranquia = key.IdFranquia,
                IdCota = key.IdCota,
                Anuncios = group.ToList()
            }).ToList();
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

            progresso.Mensagem($"2. Removendo {idProdutosRemover.Length} anúncios...", percentualConcluido: 10);

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
            int[] produtosParaAtualizar = anuncios.Where(_ => _.Status == StatusAnuncioPortal.Desatualizado)
                                                  .Select(_ => _.AnuncioCota.IdProduto)
                                                  .ToArray();

            if (produtosParaAtualizar.Length == 0)
                return new List<AnuncioAtualizacao>();

            progresso.Mensagem($"3. Obtendo dados dos {produtosParaAtualizar.Length} produtos.", percentualConcluido: 20);

            IEnumerable<Produto> produtos = _serviceProvider.ObterServico<IProdutoService>().ObterDados(produtosParaAtualizar.ToArray(), progresso).ToList();

            List<AnuncioAtualizacao> atualizacoes = new();

            if (!produtos.Any())
                return atualizacoes;

            foreach (Produto imovel in produtos)
            {
                imovel.CodigoClientePortal = anuncios.FirstOrDefault(_ => _.AnuncioCota.IdProduto == imovel.Dados.IdProduto).AnuncioCota.CodigoClientePortal;
                atualizacoes.Add(new AnuncioAtualizacao(portal, imovel.Dados.IdProduto, idEmpresa, AtualizacaoAcao.Atualizacao));
            }

            progresso.Mensagem("3. Atualizando produtos...", percentualConcluido: 30);
            atualizador.InserirAtualizarProdutos(produtos, progresso: progresso);

            return atualizacoes;
        }

        private bool RegistrarAtualizacao(List<AnuncioAtualizacao> atualizacoes,  CancellationToken cancellationToken, IProgresso? progresso = null)
        {
            if(atualizacoes.Nenhum())
                return true;

            progresso.Mensagem("4. Registrando o status...", percentualConcluido: 80);

            var handler = _serviceProvider.ObterServico<IRequestHandler<RegistroAtualizacoesCommand, bool>>();

            return handler.Handle(new RegistroAtualizacoesCommand(atualizacoes, progresso), cancellationToken).Result;
        }
    }
}
