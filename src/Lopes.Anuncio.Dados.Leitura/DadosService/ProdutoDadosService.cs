using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Dados.Leitura.Context;
using Microsoft.EntityFrameworkCore;
using Lopes.Anuncio.Application.Interfaces.DadosService;

namespace Lopes.Anuncio.Dados.Leitura.DadosService
{
    public class ProdutoDadosService : IProdutoDadosService
    {
        protected readonly DbProdutoLeituraContext Db;

        public ProdutoDadosService(DbProdutoLeituraContext context)
        {
            Db = context;
        }



        public IEnumerable<DadosPrincipais> ObterDados(int[] idProdutos)
        {
            return Db.Produtos.AsNoTracking()
                              .Where(_ => idProdutos.Contains(_.IdProduto))
                              .ToList();
        }

        public int[] ObterFranquias(int idProdutos)
        {
            return Db.ProdutoFranquias.AsNoTracking()
                                      .Where(_ => _.IdProduto == idProdutos)
                                      .Select(_ => _.IdFranquia)
                                      .Distinct()
                                      .ToArray();
        }

        public IEnumerable<Caracteristica> ObterCaracteristicas(int[] idProdutos)
        {
            string produtosStr = string.Join(",", idProdutos);
            return Db.ProdutoCaracteristicas.FromSqlRaw("dbo.ProdutoCaracteristicasLote {0}", produtosStr).ToList();
        }

        public IDictionary<int, string[]> ObterUrlTourVirtuais(int[] idProdutos)
        {
            return Db.TourVirtuais.AsNoTracking()
                                  .Where(_ => idProdutos.Contains(_.IdProduto))
                                  .ToList()
                                  .GroupBy(_ => _.IdProduto)
                                  .ToDictionary(_ => _.Key, _ => _.Select(_ => _.Url).ToArray());
        }

        public IDictionary<int, string[]> ObterUrlVideos(int[] idProdutos)
        {
            return Db.ProdutoVideos.AsNoTracking()
                                  .Where(_ => idProdutos.Contains(_.IdProduto))
                                  .ToList()
                                  .GroupBy(_ => _.IdProduto)
                                  .ToDictionary(_ => _.Key, _ => _.Select(_ => _.Url).ToArray());
        }

        public IEnumerable<Foto> ObterFotos(int[] idProdutos)
        {
            return Db.Fotos.AsNoTracking()
                           .Where(_ => idProdutos.Contains(_.IdProduto))
                           .ToList();
        }
    }
}
