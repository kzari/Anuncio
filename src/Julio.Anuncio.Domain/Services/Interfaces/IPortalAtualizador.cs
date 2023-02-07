using Julio.Domain.Commons;
using Julio.Anuncio.Domain.Enums;
using Julio.Anuncio.Domain.Models.DadosProduto;

namespace Julio.Anuncio.Domain.Services
{
    public interface IPortalAtualizadorApi : IPortalAtualizador
    {

    }

    public interface IPortalAtualizadorXml : IPortalAtualizador
    {
    }

    public interface IPortalAtualizadorFactory
    {
        IPortalAtualizador ObterAtualizador(Portal portal, int idEmpresa);
    }
    public interface IPortalAtualizador
    {
        void InserirAtualizarProdutos(IEnumerable<Produto> dados, bool removerSeExiste = false, IProgresso progresso = null);
        void RemoverProdutos(int[] idProduto, IProgresso progresso = null);
        bool ProdutoNoPortal(int idProduto);
        IEnumerable<int> ObterIdProdutosNoPortal();
    }
}
