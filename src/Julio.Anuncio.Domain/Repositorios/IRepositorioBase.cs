
namespace Julio.Anuncio.Domain.Reposities
{
    public interface IRepositorioBase<TEntidade> where TEntidade : class
    {
        void Criar(TEntidade entidade);
        void Alterar(TEntidade entidade);
        void Excluir(int id);
    }
}
