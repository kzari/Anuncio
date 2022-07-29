using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lopes.SC.Infra.Data.Repositories
{
    public class ImovelRepository : IImovelRepository
    {
        protected readonly DbProdutoContext Db;

        public ImovelRepository(DbProdutoContext context)
        {
            Db = context;
        }

        public DadosPrincipais ObterDadosImovel(int idImovel) => ObterDadosImoveis(new int[] { idImovel }).FirstOrDefault();
        public IEnumerable<DadosPrincipais> ObterDadosImoveis(int[] idImoveis)
        {
            return Db.Imoveis.Where(_ => idImoveis.Contains(_.IdImovel));
        }

        public int[] ObterEmpresasImovel(int idImovel)
        {
            return Db.ImovelEmpresas.Where(_ => _.IdImovel == idImovel)
                                    .Select(_=> _.IdEmpresa)
                                    .Distinct()
                                    .ToArray();
        }

        public IEnumerable<Caracteristica> ObterCaracteristicas(int[] idImoveis)
        {
            string imoveisStr = string.Join(",", idImoveis);
            return Db.ImovelCaracteristicas.FromSqlRaw("dbo.ImovelCaracteristicasLote {0}", imoveisStr).ToList();
        }

        public IDictionary<int, string[]> ObterUrlTourVirtuais(int[] idImoveis)
        {
            return Db.TourVirtuais.Where(_ => idImoveis.Contains(_.IdImovel))
                                  .ToList()
                                  .GroupBy(_ => _.IdImovel)
                                  .ToDictionary(_ => _.Key, _ => _.Select(_ => _.Url).ToArray());
        }

        public IDictionary<int, string[]> ObterUrlVideos(int[] idImoveis)
        {
            return Db.ImovelVideos.Where(_ => idImoveis.Contains(_.IdImovel))
                                  .ToList()
                                  .GroupBy(_ => _.IdImovel)
                                  .ToDictionary(_ => _.Key, _ => _.Select(_ => _.Url).ToArray());
        }
    }
}
