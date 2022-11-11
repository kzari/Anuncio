using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Dados.Leitura.Context;
using Lopes.Anuncio.Application.DadosService;

namespace Lopes.Anuncio.Dados.Leitura.DadosService
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