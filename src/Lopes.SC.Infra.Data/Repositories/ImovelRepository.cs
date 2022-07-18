using Lopes.SC.ExportacaoAnuncio.Domain.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
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
            return Db.ImovelCaracteristicas.FromSqlRaw("dbo.ImovelCaracteristicas {0}", idImovel);
        }
    }
}
