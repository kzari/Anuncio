using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Application.Services
{
    public class AnuncioAppService : IAnuncioAppService
    {
        private readonly IAnuncioRepository _anuncioRepository;

        public AnuncioAppService(IAnuncioRepository anuncioRepository)
        {
            _anuncioRepository = anuncioRepository;
        }


        public IEnumerable<Anuncio> ObterAnunciosPorCotas(int[] idCotas)
        {
            return _anuncioRepository.ObterPorCotas(idCotas);
        }

        public IEnumerable<Anuncio> ObterAnunciosPorImoveis(int[] idImoveis)
        {
            return _anuncioRepository.ObterPorImoveis(idImoveis);
        }

        public IEnumerable<Anuncio> ObterAnunciosPorPortais(Portal[] portais)
        {
            return _anuncioRepository.ObterPorPortais(portais);
        }
    }
}
