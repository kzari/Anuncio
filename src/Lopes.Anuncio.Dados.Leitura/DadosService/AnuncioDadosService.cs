using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Dados.Leitura.Context;
using Microsoft.EntityFrameworkCore;
using Lopes.Anuncio.Application.DadosService;

namespace Lopes.Anuncio.Dados.Leitura.DadosService
{
    public class AnuncioDadosService : DadosServiceBase<AnuncioCota>, IAnuncioDadosAppService
    {
        public AnuncioDadosService(DbProdutoLeituraContext context) : base(context)
        {
        }

        public IEnumerable<AnuncioCota> Obter(AnuncioCotaRequest request)
        {
            IQueryable<AnuncioCota> query = base.ObterTodos();

            if (request.IdProdutos != null && request.IdProdutos.Any())
                query = query.Where(_ => request.IdProdutos.Contains(_.IdProduto));

            if (request.IdCotas != null && request.IdCotas.Any())
                query = query.Where(_ => request.IdCotas.Contains(_.IdCota));

            if (request.Portais != null && request.Portais.Any())
                query = query.Where(_ => request.Portais.Contains(_.Portal));

            return query.ToList();
        }
    }
}
