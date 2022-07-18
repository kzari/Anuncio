
namespace Lopes.SC.ExportacaoAnuncio.Domain.Reposities
{
    public interface IRepository<TEntidade> where TEntidade : class
    {
        IQueryable<TEntidade> ObterTodos();
        TEntidade ObterPorId(int id);
        void Criar(TEntidade entidade);
        void Alterar(TEntidade entidade);
        void Excluir(int id);
    }
}
