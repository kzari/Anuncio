using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;
using Lopes.SC.Infra.XML;

namespace Lopes.SC.Infra.IoC
{
    public class PortalAtualizadorFactory : IPortalAtualizadorFactory
    {
        private readonly IDictionary<Portal,IEnumerable<PortalCaracteristica>> _portaisCaracteristicas;
        private readonly IServiceProvider _serviceProvider;

        public PortalAtualizadorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _portaisCaracteristicas = new Dictionary<Portal, IEnumerable<PortalCaracteristica>>();
        }



        private IEnumerable<EmpresaApelidoPortal> apelidos;
        public IEnumerable<EmpresaApelidoPortal> Apelidos => apelidos ??= ((IEmpresaApelidoPortalRepository)_serviceProvider.GetService(typeof(IEmpresaApelidoPortalRepository))).Obter();

        public IPortalAtualizador ObterAtualizador(Portal portal, int idEmpresa)
        {
            switch (portal)
            {
                case Portal.ImovelWeb:
                    return new ImovelWebApi();

                default:
                    {
                        // TODO: Obter de config
                        string caminhoPastaXmls = "C:/temp/portais_novo";

                        IEnumerable<PortalCaracteristica> portalCaracteristicas = ObterCaracteristicasPortal(portal);
                        string apelidoEmpresa = ObterApelidoEmpresa(idEmpresa);

                        return new PortalXMLBuilder(caminhoPastaXmls, portalCaracteristicas, apelidoEmpresa, portal, idEmpresa);
                    }
            }
        }


        public string ObterApelidoEmpresa(int idEmpresa)
        {
            return Apelidos.FirstOrDefault(_ => _.IdEmpresa == idEmpresa)?.Apelido ?? string.Empty;
        }

        public IEnumerable<PortalCaracteristica> ObterCaracteristicasPortal(Portal portal)
        {
            IEnumerable<PortalCaracteristica> caracteristicas;

            lock (_portaisCaracteristicas)
            {
                if(_portaisCaracteristicas.TryGetValue(portal, out caracteristicas))
                    return caracteristicas;

                IPortalCaracteristicaRepository portalCaracteristicaRepository = (IPortalCaracteristicaRepository)_serviceProvider.GetService(typeof(IPortalCaracteristicaRepository));
                caracteristicas = portalCaracteristicaRepository.Obter(portal) ?? new List<PortalCaracteristica>();

                lock (_portaisCaracteristicas)
                    _portaisCaracteristicas.Add(portal, caracteristicas);
            }

            return caracteristicas;
        }
    }
}
