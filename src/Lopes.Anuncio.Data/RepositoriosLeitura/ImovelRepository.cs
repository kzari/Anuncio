using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Models.Imovel;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Infra.Data.Repositories
{
    public class ImovelRepository : IImovelRepository
    {
        protected readonly DbProdutoContext Db;

        public ImovelRepository(DbProdutoContext context)
        {
            Db = context;
        }
        
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

        public IEnumerable<Fotos> ObterFotos(int[] idImoveis)
        {
            return Db.ImovelImagens.Where(_ => idImoveis.Contains(_.IdImovel)).ToList();
        }
    }
}
