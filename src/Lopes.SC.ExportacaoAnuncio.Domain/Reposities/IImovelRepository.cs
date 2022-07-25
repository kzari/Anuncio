using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Reposities
{
    public interface IImovelRepository
    {
        IEnumerable<Caracteristica> ObterCaracteristicas(int idImovel);
        IEnumerable<DadosPrincipais> ObterDadosImoveis(int[] idImoveis);
        DadosPrincipais ObterDadosImovel(int idImovel);
        int[] ObterEmpresasImovel(int idImovel);
        IEnumerable<string> ObterUrlTourVirtuais(int idImovel);
        IEnumerable<string> ObterUrlVideos(int idImovel);
    }
}
