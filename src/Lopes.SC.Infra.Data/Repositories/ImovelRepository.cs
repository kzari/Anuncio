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

        public IEnumerable<string> ObterUrlTourVirtual(int idUnidade)
        {
            return  Select<string>($"SELECT UNTV_nm_url_tour_virtual FROM tb_UNTV_unidade_tour_virtual WHERE UNPR_cd_unidade_pronta = {idUnidade}").ToList();
        }

        public IEnumerable<string> ObterUrlVideosCache(int idUnidade)
        {
            string key = $"URLVideos_{idUnidade}";
            return _cache.Fetch(key, ObterUrlVideos, idUnidade, new NewntonsoftSerializerWrapper());
        }
        public IEnumerable<string> ObterUrlVideos(int idUnidade)
        {
            return Select<string>($"SELECT UNVI_nm_url_video FROM tb_UNVI_unidade_video WHERE UNPR_cd_unidade_pronta = {idUnidade}").ToList();
        }

    }
}
