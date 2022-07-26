using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;
using Lopes.SC.Infra.Data.Context;
using Lopes.SC.Infra.Data.Repositories;
using Lopes.SC.Infra.XML;

namespace Lopes.SC.Infra.IoC
{
    public class PortalAtualizadorFactory : IPortalAtualizadorFactory
    {
        public IPortalAtualizador ObterAtualizador(Portal portal, int idEmpresa)
        {
            switch (portal)
            {
                case Portal.ImovelWeb:
                    return new ImovelWebApi();

                default:
                    {
                        // TODO: Obter por DI
                        var repo = new EmpresaApelidoPortalRepository(new DbLopesnetContext());
                        // TODO: Obter de config
                        string caminhoPastaXmls = "C:/temp/portais_novo";
                        return new PortalXMLBuilder(caminhoPastaXmls, repo, portal, idEmpresa);
                    }
            }
        }
    }
}
