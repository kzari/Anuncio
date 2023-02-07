using Julio.Anuncio.Domain.Commands.Requests;
using Julio.Anuncio.Domain.ObjetosValor;
using Julio.Anuncio.Dados.Leitura.Context;
using Microsoft.EntityFrameworkCore;
using Julio.Anuncio.Application.DadosService;

namespace Julio.Anuncio.Dados.Leitura.DadosService
{
    public class AnuncioDadosService : DadosServiceBase<AnuncioCota>, IAnuncioDadosService
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
