using Lopes.Domain.Commons;
using Lopes.Anuncio.Domain.Models.Imovel;

namespace Lopes.Anuncio.Application.Interfaces
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