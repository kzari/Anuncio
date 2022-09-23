using Lopes.Domain.Commons;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Infra.Data.Repositories;
using Lopes.Anuncio.Domain.Entidades;
using Lopes.Anuncio.Repositorio.Context;

namespace Lopes.Anuncio.Repositorio.Repositorios
{
    public class AnuncioStatusRepositorio : RepositorioBase<AnuncioAtualizacao>, IAnuncioStatusRepositorio
    {
        public AnuncioStatusRepositorio(DbProdutoContext context) : base(context)
        {
        }

        public void Criar(AnuncioAtualizacao entidade, bool salvarAlteracoes = true)
        {
            base.Criar(entidade);
            if (salvarAlteracoes)
                SalvarAlteracoes();
        }

        public void Criar(IEnumerable<AnuncioAtualizacao> entidades, IProgresso? progresso = null)
        {
            int i = 0;
            foreach (AnuncioAtualizacao entidade in entidades)
            {
                i++;
                Criar(entidade, salvarAlteracoes: false);

                if (i % 200 == 0)
                {
                    progresso?.Mensagem($"Registrando status dos anúncios... {i} de {entidades.Count()}", i);

                    SalvarAlteracoes();
                }
            }
            SalvarAlteracoes();
            progresso?.Mensagem("Registro de atualização concluído", i);
        }
    }
}
