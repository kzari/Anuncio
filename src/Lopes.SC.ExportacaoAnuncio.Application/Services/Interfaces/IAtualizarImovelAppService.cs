using Lopes.SC.Domain.Commons;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Application.Interfaces
{
    public interface IAtualizarAnunciosAppService
    {
        void AtualizarPorCotas(int[] idCotas, ILogger log);
        void AtualizarPorImoveis(int[] idImoveis, Portal? portal, ILogger log);
        void AtualizarPorPortais(Portal[] portais, ILogger log);
    }
}