using Lopes.SC.Anuncio.Domain.Enums;
using Lopes.SC.Anuncio.Domain.Reposities;
using Lopes.SC.Anuncio.Domain.Models;

namespace Lopes.SC.Anuncio.Application.Services
{
    public class AnuncioAppService : IAnuncioAppService
    {
        private readonly IAnuncioRepository _anuncioRepository;

        public AnuncioAppService(IAnuncioRepository anuncioRepository)
        {
            _anuncioRepository = anuncioRepository;
        }


        public IEnumerable<AnuncioImovel> ObterAnunciosPorCotas(int[] idCotas)
        {
            return _anuncioRepository.ObterPorCotas(idCotas);
        }

        public IEnumerable<AnuncioImovel> ObterAnunciosPorImoveis(int[] idImoveis, Portal? portal = null)
        {
            return _anuncioRepository.ObterPorImoveis(idImoveis, portal);
        }

        public IEnumerable<AnuncioImovel> ObterAnunciosPorPortais(Portal[] portais)
        {
            return _anuncioRepository.ObterPorPortais(portais);
        }
    }
}
