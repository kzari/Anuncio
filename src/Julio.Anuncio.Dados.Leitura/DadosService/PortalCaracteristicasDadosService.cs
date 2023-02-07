using Julio.Anuncio.Domain.Enums;
using Julio.Anuncio.Domain.ObjetosValor;
using Julio.Anuncio.Dados.Leitura.Context;
using Julio.Anuncio.Application.DadosService;

namespace Julio.Anuncio.Dados.Leitura.DadosService
{
    public class PortalCaracteristicasDadosService : DadosServiceBase<PortalCaracteristica>, IPortalCaracteristicaDadosService
    {
        public PortalCaracteristicasDadosService(DbProdutoLeituraContext context) : base(context)
        {
        }

        public IEnumerable<PortalCaracteristica> Obter(Portal portal)
        {
            return ObterTodos().Where(_ => _.Portal == portal).ToList();
        }
    }
}