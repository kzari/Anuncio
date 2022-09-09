using Lopes.Domain.Commons;
using Lopes.Anuncio.Domain.Enums;

namespace Lopes.Anuncio.Application.Interfaces
{
    public interface IAtualizarAnunciosAppService
    {
        void AtualizarPorCotas(int[] idCotas, ILogger log);
        void AtualizarPorImoveis(int[] idImoveis, Portal? portal, ILogger log);
        void AtualizarPorPortais(Portal[] portais, ILogger log);
    }
}