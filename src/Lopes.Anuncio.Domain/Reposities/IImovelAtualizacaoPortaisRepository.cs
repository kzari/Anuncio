using Lopes.Domain.Commons;
using Lopes.Anuncio.Domain.Commands.Requests;

namespace Lopes.Anuncio.Domain.Reposities
{
    public interface IAnuncioStatusRepositorioGravacao
    {
        void AtualizarOuAdicionar(AtualizarStatusAnuncioRequest model, bool salvarAlteracoes = true);
        void AtualizarOuAdicionar(IEnumerable<AtualizarStatusAnuncioRequest> model, IProgresso progresso = null);
    }
}
