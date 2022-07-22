using Lopes.SC.AnuncioXML.Domain.Models;

namespace Lopes.SC.XML.Domain
{
    public interface IPortalXML
    {
        Elemento CriarElementoImovel(Dados dados);
        Elemento CriarElementoCabecalho();
    }
}