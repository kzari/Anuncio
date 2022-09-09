using Lopes.SC.Anuncio.Domain.Imovel;
using Lopes.SC.Anuncio.Domain.Models;
using Lopes.SC.Anuncio.Domain.Reposities;

namespace Lopes.SC.Anuncio.Domain.Testes.Mocks
{
    public class ImovelRepositoryMock : IImovelRepository
    {
        public IEnumerable<Caracteristica> ObterCaracteristicas(int[] idImoveis)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DadosPrincipais> ObterDadosImoveis(int[] idImoveis)
        {
            throw new NotImplementedException();
        }

        public DadosPrincipais ObterDadosImovel(int idImovel)
        {
            throw new NotImplementedException();
        }

        public int[] ObterEmpresasImovel(int idImovel)
        {
            return new[] { 1 };
        }

        public IEnumerable<Fotos> ObterFotos(int[] idImoveis)
        {
            throw new NotImplementedException();
        }

        public IDictionary<int, string[]> ObterUrlTourVirtuais(int[] idImoveis)
        {
            throw new NotImplementedException();
        }

        public IDictionary<int, string[]> ObterUrlVideos(int[] idImoveis)
        {
            throw new NotImplementedException();
        }
    }
}