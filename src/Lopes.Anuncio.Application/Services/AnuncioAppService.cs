using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Models;

namespace Lopes.Anuncio.Application.Services
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
