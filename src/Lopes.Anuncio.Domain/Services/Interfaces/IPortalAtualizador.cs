using Lopes.Domain.Commons;
using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models.Imovel;

namespace Lopes.Anuncio.Domain.Services
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
        void InserirAtualizarImoveis(IEnumerable<DadosImovel> dados, bool removerSeExiste = false, IProgresso progresso = null);
        void RemoverImoveis(int[] idImovel, IProgresso progresso = null);
        bool ImovelNoPortal(int idImovel);
        IEnumerable<int> ObterIdImoveisNoPortal();
    }
}
