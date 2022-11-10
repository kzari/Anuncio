using Lopes.Domain.Commons;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Acesso.Data.Repositories;
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

                if (i % 1000 == 0)
                {
                    if(progresso != null)
                        progresso.Mensagem($"Registrando status dos anúncios... {i} de {entidades.Count()}", i);

                    SalvarAlteracoes();
                }
            }
            SalvarAlteracoes();
            if (progresso != null)
                progresso.Mensagem($"Registro de atualização concluído", i);
        }




        //public void AtualizarOuAdicionar(AnuncioAtualizacao entidade, bool salvarAlteracoes = true)
        //{
        //    AnuncioAtualizacao? registro = ObterTodos().FirstOrDefault(_ => _.IdPortal == entidade.IdPortal &&
        //                                                                    _.IdProduto == entidade.IdProduto &&
        //                                                                    _.IdEmpresa == entidade.IdEmpresa);
        //    if (registro == null)
        //        Criar(entidade);
        //    else
        //    {
        //        registro.Data = entidade.Data;
        //        Alterar(registro);
        //    }
        //    if (salvarAlteracoes)
        //        SalvarAlteracoes();
        //}
        //public void AtualizarOuAdicionar(IEnumerable<AnuncioAtualizacao> entidades, IProgresso progresso = null)
        //{
        //    int i = 0;
        //    foreach (AnuncioAtualizacao entidade in entidades)
        //    {
        //        i++;
        //        AtualizarOuAdicionar(entidade, salvarAlteracoes: false);

        //        if (i % 1000 == 0)
        //        {
        //            progresso.Mensagem($"Atualizando status do anúncio/imóvel. {i} de {entidades.Count()}", i);
        //            SalvarAlteracoes();
        //        }
        //    }
        //    SalvarAlteracoes();
        //    if (progresso != null)
        //        progresso.Mensagem($"Atualização de status concluída", i);
        //}
    }
}
