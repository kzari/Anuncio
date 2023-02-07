using Julio.Anuncio.Domain.ObjetosValor;
using Julio.Anuncio.Dados.Leitura.Context;
using Julio.Anuncio.Application.DadosService;

namespace Julio.Anuncio.Dados.Leitura.DadosService
{
    public class FranquiaApelidoPortalDadosService : DadosServiceBase<FranquiaApelido>, IFranquiaApelidoPortalDadosService
    {
        public FranquiaApelidoPortalDadosService(DbLopesnetLeituraContext context) : base(context)
        {
        }

        public IEnumerable<FranquiaApelido> Obter() => ObterTodos().ToList();
    }
}
