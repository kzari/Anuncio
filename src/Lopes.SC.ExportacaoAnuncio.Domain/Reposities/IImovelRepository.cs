using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Reposities
{
    public interface IImovelRepository
    {
        IEnumerable<ImovelCaracteristica> ObterCaracteristicas(int idImovel);
        IEnumerable<DadosImovel> ObterDadosImoveis(int[] idImoveis);
        DadosImovel ObterDadosImovel(int idImovel);
        int[] ObterEmpresasImovel(int idImovel);
    }
}
