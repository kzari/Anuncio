using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Application.Interfaces
{
    public interface IAtualizarAnunciosAppService
    {
        void AtualizarPorCotas(int[] idCotas);
        void AtualizarPorImoveis(int[] idImoveis);
        void AtualizarPorPortais(Portal[] portais);
    }
}