using Lopes.SC.AnuncioXML.Domain.Enums;

namespace Lopes.SC.AnuncioXML.Application.Services
{
    public interface IAtualizarAnunciosAppService
    {
        void AtualizarPorCotas(int[] idCotas);
        void AtualizarPorImoveis(int[] idImoveis);
        void AtualizarPorPortais(Portal[] portais);
    }
}