using Lopes.Domain.Commons;
using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Domain.Imovel;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Reposities;

namespace Lopes.Anuncio.Application.Services
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

        public IEnumerable<DadosImovel> ObterDadosImovel(int[] idImoveis, IProgresso progresso = null)
        {
            if (progresso != null)
                progresso.Atualizar($"Obtendo dados principais de {idImoveis.Count()} imóveis.");

            List<DadosPrincipais> dadosPrincipais = _imovelRepository.ObterDadosImoveis(idImoveis).ToList();

            int[] idImoveisResgatados = dadosPrincipais.Select(_ => _.IdImovel).ToArray();
            
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
            foreach (DadosPrincipais dados in dadosPrincipais)
            {
                DadosImovel imovel = new (dados);

                imovel.Caracteristicas = caracteristicas.Where(_ => _.IdImovel == dados.IdImovel);
                imovel.UrlTourVirtuais = urlTours.Where(_ => _.Key == dados.IdImovel).SelectMany(_ => _.Value);
                imovel.UrlVideos       = urlVideos.Where(_ => _.Key == dados.IdImovel).SelectMany(_ => _.Value);
                imovel.Imagens         = fotos.Where(_ => _.IdImovel == dados.IdImovel);

                yield return imovel;
            }
        }
    }
}