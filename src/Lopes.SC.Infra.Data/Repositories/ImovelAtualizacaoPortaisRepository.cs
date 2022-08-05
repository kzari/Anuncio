using Lopes.SC.Domain.Commons;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.Infra.Data.Context;

namespace Lopes.SC.Infra.Data.Repositories
{
    public class ImovelAtualizacaoPortaisRepository : Repository<AnuncioAtualizacao>, IImovelAtualizacaoPortaisRepository
    {
        public ImovelAtualizacaoPortaisRepository(DbProdutoContext context) : base(context)
        {
        }

        public void AtualizarOuAdicionar(AnuncioAtualizacao model, bool salvarAlteracoes = true)
        {
            AnuncioAtualizacao? registro = ObterTodos().FirstOrDefault(_ => _.IdPortal == model.IdPortal &&
                                                                            _.IdImovel == model.IdImovel &&
                                                                            _.IdEmpresa == model.IdEmpresa);
            if (registro == null)
                Criar(model);
            else
            {
                registro.Data = model.Data;
                Alterar(registro);
            }
            if(salvarAlteracoes)
                SalvarAlteracoes();
        }

        public void AtualizarOuAdicionar(IEnumerable<AnuncioAtualizacao> models, IProgresso progresso = null)
        {
            int i = 0;
            foreach (var model in models)
            {
                i++;
                AtualizarOuAdicionar(model, salvarAlteracoes: false);

                if (i % 1000 == 0)
                {
                    progresso.Atualizar($"Atualizando status do anúncio/imóvel. {i} de {models.Count()}", i);
                    SalvarAlteracoes();
                }
            }
            SalvarAlteracoes();
            if(progresso != null)
                progresso.Atualizar($"Atualização de status concluída", i);
        }
    }
}
