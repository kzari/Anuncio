using Lopes.SC.Domain.Commons;
using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
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

            imovel.Caracteristicas = _imovelRepository.ObterCaracteristicas(new[] { idImovel });
            imovel.UrlTourVirtuais = _imovelRepository.ObterUrlTourVirtuais(new[] { idImovel }).SelectMany(_ => _.Value);
            imovel.UrlVideos = _imovelRepository.ObterUrlVideos(new[] { idImovel }).SelectMany(_ => _.Value);

            //TODO: preencher: v
            //dados.Imagens = 

            return imovel;
        }

        public IEnumerable<DadosImovel> ObterDadosImovel(int[] idImoveis, IProgresso progresso = null)
        {
            if (progresso != null)
                progresso.Atualizar($"Obtendo dados principais de {idImoveis.Count()} imóveis.");

            List<DadosPrincipais> principais = _imovelRepository.ObterDadosImoveis(idImoveis).ToList();
            if (principais == null)
                return null;

            int[] idImoveisResgatados = principais.Select(_ => _.IdImovel).ToArray();
            

            if (progresso != null)
                progresso.Atualizar($"Obtendo caracteristicas de {idImoveisResgatados.Count()} imóveis.");

            List<Caracteristica>? caracteristicas = _imovelRepository.ObterCaracteristicas(idImoveisResgatados).ToList();


            if (progresso != null)
                progresso.Atualizar($"Obtendo Tour virtual e Vídeos de {idImoveisResgatados.Count()} imóveis.");

            IDictionary<int, string[]> urlTours = _imovelRepository.ObterUrlTourVirtuais(idImoveisResgatados);
            IDictionary<int, string[]> urlVideos = _imovelRepository.ObterUrlVideos(idImoveisResgatados);
            IEnumerable<Fotos> fotos = _imovelRepository.ObterFotos(idImoveisResgatados);


            if (progresso != null)
                progresso.Atualizar($"Preenchendo informações de {idImoveisResgatados.Count()} imóveis.");
            List<DadosImovel> imoveis = new List<DadosImovel>();
            foreach (DadosPrincipais dados in principais)
            {
                DadosImovel imovel = new DadosImovel(dados);

                imovel.Caracteristicas = caracteristicas.Where(_ => _.IdImovel == dados.IdImovel);
                imovel.UrlTourVirtuais = urlTours.Where(_ => _.Key == dados.IdImovel).SelectMany(_ => _.Value);
                imovel.UrlVideos       = urlVideos.Where(_ => _.Key == dados.IdImovel).SelectMany(_ => _.Value);
                imovel.Imagens         = fotos.Where(_ => _.IdImovel == dados.IdImovel);

                //TODO: preencher imagens
                //dados.Imagens = 
                imoveis.Add(imovel);
            }
            return imoveis;
        }
    }
}