using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;

namespace Lopes.SC.ExportacaoAnuncio.Application.Services
{

    public class DadosImovelAppService : IDadosImovelAppService
    {
        private readonly IImovelRepository _imovelRepository;

        
        public DadosImovelAppService(IImovelRepository imovelRepository)
        {
            _imovelRepository = imovelRepository;
        }


        public int[] ObterEmpresasImovel(int idImovel)
        {
            return _imovelRepository.ObterEmpresasImovel(idImovel);
        }

        public DadosImovel ObterDadosImovel(int idImovel)
        {
            DadosPrincipais? principais = _imovelRepository.ObterDadosImovel(idImovel);

            if (principais == null)
                return null;

            DadosImovel imovel = new DadosImovel(principais);

            imovel.Caracteristicas = _imovelRepository.ObterCaracteristicas(idImovel).ToList();
            imovel.UrlTourVirtuais = _imovelRepository.ObterUrlTourVirtuais(idImovel).ToList();
            imovel.UrlVideos = _imovelRepository.ObterUrlVideos(idImovel).ToList();
            //TODO: preencher: v
            //dados.Imagens = 

            return imovel;
        }
    }
}