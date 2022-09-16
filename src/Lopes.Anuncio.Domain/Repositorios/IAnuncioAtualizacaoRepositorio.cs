using Lopes.Domain.Commons;
using Lopes.Anuncio.Domain.Entidades;

namespace Lopes.Anuncio.Domain.Reposities
{
    public interface IAnuncioStatusRepositorio
    {
        void Criar(AnuncioAtualizacao entidade, bool salvarAlteracoes = true);
        void Criar(IEnumerable<AnuncioAtualizacao> entidade, IProgresso? progresso = null);
        //void AtualizarOuAdicionar(AnuncioAtualizacao entidade, bool salvarAlteracoes = true);
        //void AtualizarOuAdicionar(IEnumerable<AnuncioAtualizacao> entidades, IProgresso progresso = null);
    }
}
