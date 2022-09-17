using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Dados.Leitura.Context;
using Lopes.Anuncio.Application.DadosService;

namespace Lopes.Anuncio.Dados.Leitura.DadosService
{
    public class FranquiaApelidoPortalDadosService : DadosServiceBase<FranquiaApelido>, IFranquiaApelidoPortalDadosAppService
    {
        public FranquiaApelidoPortalDadosService(DbLopesnetLeituraContext context) : base(context)
        {
        }

        public IEnumerable<FranquiaApelido> Obter() => ObterTodos().ToList();
    }
}
