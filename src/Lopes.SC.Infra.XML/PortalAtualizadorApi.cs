using Lopes.SC.Domain.Commons;
using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;

namespace Lopes.SC.Infra.XML
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

        public void InserirAtualizarImoveis(IEnumerable<DadosImovel> dados, bool removerSeExiste = false, IProgresso progresso = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> ObterIdImoveisNoPortal()
        {
            throw new NotImplementedException();
        }

        public void RemoverImoveis(int[] idImovel, IProgresso progresso = null)
        {
            throw new NotImplementedException();
        }
    }
}
