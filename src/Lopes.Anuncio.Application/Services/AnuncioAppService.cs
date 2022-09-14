using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Application.Services
{
    public class AnuncioAppService : IAnuncioAppService
    {
        private readonly IAnuncioRepository _anuncioRepository;

        public AnuncioAppService(IAnuncioRepository anuncioRepository)
        {
            _anuncioRepository = anuncioRepository;
        }

        public IEnumerable<AnuncioCota> ObterAnunciosPorCotas(int[] idCotas)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AnuncioCota> ObterAnunciosPorImoveis(int[] idImoveis, Portal? portal = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AnuncioCota> ObterAnunciosPorPortais(Portal[] portais)
        {
            throw new NotImplementedException();
        }
    }
}
