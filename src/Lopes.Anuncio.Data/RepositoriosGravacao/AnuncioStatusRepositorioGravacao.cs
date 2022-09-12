using Lopes.Domain.Commons;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Infra.Data.Context;
using Lopes.Infra.Data.Repositories;
using Lopes.Anuncio.Domain.Commands.Requests;

namespace Lopes.Infra.Data.RepositoriosGravacao
{
    public class AnuncioStatusRepositorioGravacao : Repository<AnuncioAtualizacao>, IAnuncioStatusRepositorioGravacao
    {
        public AnuncioStatusRepositorioGravacao(DbProdutoContext context) : base(context)
        {
        }

        public void AtualizarOuAdicionar(AtualizarStatusAnuncioRequest request, bool salvarAlteracoes = true)
        {
            AnuncioAtualizacao? registro = ObterTodos().FirstOrDefault(_ => _.IdPortal == request.IdPortal &&
                                                                            _.IdImovel == request.IdImovel &&
                                                                            _.IdEmpresa == request.IdEmpresa);
            if (registro == null)
                Criar(new AnuncioAtualizacao(request.IdPortal, request.IdImovel, request.IdEmpresa, request.Acao, request.Data));
            else
            {
                registro.Data = request.Data;
                Alterar(registro);
            }
            if (salvarAlteracoes)
                SalvarAlteracoes();
        }

        public void AtualizarOuAdicionar(IEnumerable<AtualizarStatusAnuncioRequest> requests, IProgresso progresso = null)
        {
            int i = 0;
            foreach (AtualizarStatusAnuncioRequest request in requests)
            {
                i++;
                AtualizarOuAdicionar(request, salvarAlteracoes: false);

                if (i % 1000 == 0)
                {
                    progresso.Atualizar($"Atualizando status do anúncio/imóvel. {i} de {requests.Count()}", i);
                    SalvarAlteracoes();
                }
            }
            SalvarAlteracoes();
            if (progresso != null)
                progresso.Atualizar($"Atualização de status concluída", i);
        }
    }
}
