using Lopes.Anuncio.Dados.Leitura.Context;
using Lopes.Botmaker.Application.DadosServices;
using Lopes.Botmaker.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Anuncio.Dados.Leitura.DadosService
{
    public class IntegracaoBotmakerDadosService : IIntegracaoBotmakerDadosService
    {
        private readonly UsuarioLeituraContext Contexto;

        public IntegracaoBotmakerDadosService(UsuarioLeituraContext context)
        {
            Contexto = context;
        }


        public IEnumerable<DadosUsuarioDTO> ObterUsuariosIntegracao(params int[] idsEmpresas) => ObterUsuariosIntegracao(new string[] { }, idsEmpresas);

        public IEnumerable<DadosUsuarioDTO> ObterUsuariosIntegracao(string[] emails, params int[] idsEmpresas)
        {
            string idEmpresasParametro = idsEmpresas != null && idsEmpresas.Any() ? string.Join(",", idsEmpresas) : string.Empty;
            string emailsParametro = emails != null && emails.Any() ? string.Join(",", emails) : string.Empty;


            var aaa = Contexto.UsuariosIntegracaoBotmaker.FromSqlRaw($"dbo.usp_sel_usuarios_integracao_botmaker").ToList();
            return aaa;

            //return Contexto.UsuariosIntegracaoBotmaker.FromSqlRaw($"dbo.usp_sel_usuarios_integracao_botmaker @idEmpresas, @emails", idEmpresasParametro, emailsParametro).ToList();
        }
    }
}
