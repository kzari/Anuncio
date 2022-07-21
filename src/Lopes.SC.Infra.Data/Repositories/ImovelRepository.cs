using Lopes.SC.ExportacaoAnuncio.Domain.Models;
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

        public DadosImovel ObterDadosImovel(int idImovel) => ObterDadosImoveis(new int[] { idImovel }).FirstOrDefault();
        public IEnumerable<DadosImovel> ObterDadosImoveis(int[] idImoveis)
        {
            return Db.Imoveis.Where(_ => idImoveis.Contains(_.IdImovel));
        }

        public int[] ObterEmpresasImovel(int idImovel)
        {
            return Db.ImovelEmpresas.Where(_ => _.IdImovel == idImovel)
                                    .Select(_=> _.IdEmpresa)
                                    .ToArray();
        }

        public IEnumerable<ImovelCaracteristica> ObterCaracteristicas(int idImovel)
        {
            return Db.ImovelCaracteristicas.FromSqlRaw("dbo.ImovelCaracteristicas {0}", idImovel).ToList();
        }

        public IEnumerable<string> ObterUrlTourVirtuais(int idImovel)
        {
            return Db.TourVirtuais.Where(_ => _.IdImovel == idImovel)
                                  .Select(_ => _.Url)
                                  .ToList();
        }

        public IEnumerable<string> ObterUrlVideos(int idUnidade)
        {
            return Db.ImovelVideos.Where(_ => _.IdImovel == idUnidade)
                                  .Select(_ => _.Url)
                                  .ToList();
        }
    }
}
