using Lopes.SC.ExportacaoAnuncio.Domain.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.Infra.Data.Context;

namespace Lopes.SC.Infra.Data.Repositories
{
    public class ImovelAtualizacaoPortaisRepository : Repository<ImovelAtualizacaoPortais>, IImovelAtualizacaoPortaisRepository
    {
        public ImovelAtualizacaoPortaisRepository(DbProdutoContext context) : base(context)
        {
        }

        public void AtualizarOuAdicionar(ImovelAtualizacaoPortais model)
        {
            ImovelAtualizacaoPortais? registro = GetAll().FirstOrDefault(_ => _.IdPortal == model.IdPortal &&
                                                                              _.IdImovel == model.IdImovel &&
                                                                              _.IdEmpresa == model.IdEmpresa);
            if(registro == null)
                Add(model);
            else
            {
                registro.DataRemocao = model.DataRemocao;
                registro.DataAtualizacao = model.DataAtualizacao;
                Update(registro);
            }
            SaveChanges();
        }
    }
}
