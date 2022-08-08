using Lopes.SC.Domain.Commons;
using Lopes.SC.Anuncio.Domain.Models;

namespace Lopes.SC.Anuncio.Domain.Reposities
{
    public interface IImovelAtualizacaoPortaisRepository
    {
        void AtualizarOuAdicionar(AnuncioAtualizacao model, bool salvarAlteracoes = true);
        void AtualizarOuAdicionar(IEnumerable<AnuncioAtualizacao> model, IProgresso progresso = null);
    }
}
