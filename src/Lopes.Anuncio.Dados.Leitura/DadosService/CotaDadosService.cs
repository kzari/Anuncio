using Lopes.Anuncio.Application.DadosService;
using Lopes.Anuncio.Dados.Leitura.Context;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Dados.Leitura.DadosService
{
    public class CotaDadosService : DadosServiceBase<CotaResumo>, ICotaDadosService
    {
        public CotaDadosService(DbProdutoLeituraContext context) : base(context)
        {
        }

        public IEnumerable<CotaResumo> Obter(CotaResumoRequest request)
        {
            IQueryable<CotaResumo> query = ObterTodos();

            if (request.IdFranquias != null && request.IdFranquias.Any())
                query = query.Where(_ => request.IdFranquias.Contains(_.IdFranquia));

            if (request.IdCotas != null && request.IdCotas.Any())
                query = query.Where(_ => request.IdCotas.Contains(_.IdCota));

            if (request.Portais != null && request.Portais.Any())
                query = query.Where(_ => request.Portais.Contains(_.Portal));

            return query.ToList();
        }
    }
}
