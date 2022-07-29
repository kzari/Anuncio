using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Reposities
{
    public interface IImovelRepository
    {

        IEnumerable<DadosPrincipais> ObterDadosImoveis(int[] idImoveis);
        DadosPrincipais ObterDadosImovel(int idImovel);

        int[] ObterEmpresasImovel(int idImovel);


        IDictionary<int, string[]> ObterUrlTourVirtuais(int[] idImoveis);
        IDictionary<int, string[]> ObterUrlVideos(int[] idImoveis);
        IEnumerable<Caracteristica> ObterCaracteristicas(int[] idImoveis);
    }
}
