using Lopes.SC.ExportacaoAnuncio.Application.Models;

namespace Lopes.SC.ExportacaoAnuncio.Application.Interfaces
{
    public interface IDadosImovelAppService
    {
        /// <summary>
        /// Retorna os dados do imóvel
        /// </summary>
        /// <param name="idImovel"></param>
        /// <returns></returns>
        Imovel ObterDadosImovel(int idImovel);

        /// <summary>
        /// Retorna as empresas ao qual o imóvel é gerido
        /// </summary>
        /// <param name="id">Id do Imóvel</param>
        /// <returns></returns>
        int[] ObterEmpresasImovel(int idImovel);
    }
}