using Lopes.Domain.Commons;
using Lopes.Anuncio.Domain.Services;
using Lopes.Anuncio.Domain.Models.DadosProduto;

namespace Lopes.Infra.XML
{
    /// <summary>
    /// TODO: para um novo projeto
    /// </summary>
    public class ImovelWebApi : IPortalAtualizadorApi
    {
        public bool ImovelNoPortal(int idImovel)
        {
            throw new NotImplementedException();
        }

        public void InserirAtualizarProdutos(IEnumerable<Produto> dados, bool removerSeExiste = false, IProgresso progresso = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> ObterIdProdutosNoPortal()
        {
            throw new NotImplementedException();
        }

        public void RemoverProdutos(int[] idImovel, IProgresso progresso = null)
        {
            throw new NotImplementedException();
        }
    }
}
