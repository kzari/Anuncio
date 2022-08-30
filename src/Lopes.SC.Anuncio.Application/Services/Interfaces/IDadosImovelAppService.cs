using Lopes.SC.Domain.Commons;
using Lopes.SC.Anuncio.Domain.Imovel;

namespace Lopes.SC.Anuncio.Application.Interfaces
{
    public interface IDadosImovelAppService
    {
        /// <summary>
        /// Retorna os dados dos imóveis
        /// </summary>
        /// <param name="idImovel"></param>
        /// <returns></returns>
        IEnumerable<DadosImovel> ObterDadosImovel(int[] idImoveis, IProgresso progresso);

        /// <summary>
        /// Retorna as empresas ao qual o imóvel é gerido
        /// </summary>
        /// <param name="id">Id do Imóvel</param>
        /// <returns></returns>
        int[] ObterEmpresasImovel(int idImovel);
    }
}