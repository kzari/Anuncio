using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Reposities
{
    public interface IImovelAtualizacaoPortaisRepository
    {
        void AtualizarOuAdicionar(AnuncioAtualizacao model, bool salvarAlteracoes = true);
        void AtualizarOuAdicionar(IEnumerable<AnuncioAtualizacao> model);
    }
}
