using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Interfaces
{
    public interface IImovelRepository
    {
        int[] ObterEmpresasImovel(int idImovel);

        DadosImovel ObterDadosImovel(int idImovel);
        IEnumerable<DadosImovel> ObterDadosImoveis(int[] idImoveis);

        IEnumerable<ImovelCaracteristica> ObterCaracteristicas(int idImovel);
    }
}
