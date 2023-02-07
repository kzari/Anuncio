using Julio.Domain.Commons;
using Julio.Anuncio.Domain.Services;
using Julio.Anuncio.Domain.Models.DadosProduto;

namespace Julio.Acesso.XML
{
    /// <summary>
    /// TODO: para um novo projeto
    /// </summary>
    public class ProdutoWebApi : IPortalAtualizadorApi
    {
        public bool ProdutoNoPortal(int idProduto)
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

        public void RemoverProdutos(int[] idProduto, IProgresso progresso = null)
        {
            throw new NotImplementedException();
        }
    }
}
