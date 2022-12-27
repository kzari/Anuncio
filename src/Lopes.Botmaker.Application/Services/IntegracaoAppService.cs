using Lopes.Botmaker.Application.Models;
using Lopes.Domain.Commons;

namespace Lopes.Botmaker.Application.Services
{
    public interface IIntegracaoAppService
    {
        void IntegrarTudo();
    }
    public class IntegracaoAppService : IIntegracaoAppService
    {
        private readonly IBotmakerApiService _botmakerApi;

        public IntegracaoAppService(IBotmakerApiService botmakerApi)
        {
            _botmakerApi = botmakerApi;
        }

        public void IntegrarTudo()
        {
            IEnumerable<UsuarioBotmakerApi> usuariosBotmaker = ObterUsuariosNaBotmaker();

            //RemoverUsuariosDuplicados(ref usuariosBotmaker);

            //IEnumerable<UsuarioIntegracaoBotmakerDTO> usuariosParaIntegrar = _integracaoBotmakerAppService.ObterUsuariosIntegracao(emailsUsuarios, idEmpresas).ToList();

            //IncluirAtualizarUsuarios(usuariosBotmaker, usuariosParaIntegrar, removerAntesDeIncluir: false);

            //RemoverUsuarios(usuariosBotmaker, usuariosParaIntegrar);
        }


        /// <summary>
        /// Resgata as informações dos usuários que estão na Botmaker
        /// </summary>
        /// <param name="emails">Filtrar por e-mails</param>
        /// <param name="cpfs">Filtrar por CPFs</param>
        /// <param name="nomes">Filtrar por nomes</param>
        /// <returns></returns>
        private IEnumerable<UsuarioBotmakerApi> ObterUsuariosNaBotmaker(string[] emails = null, string[] cpfs = null, string[] nomes = null)
        {
            IResultado<IEnumerable<UsuarioBotmakerApi>> resultUsuariosBotmaker = _botmakerApi.ObterUsuariosNaBotmaker();
            if (resultUsuariosBotmaker.Falha)
                throw new Exception($"Ocorreu um erro ao obter os usuários na API da Botmaker: {resultUsuariosBotmaker.ErrosConcatenados()}");

            List<UsuarioBotmakerApi> usuariosBotmaker = resultUsuariosBotmaker.Dado.Where(_ => _.extraValues != null && !string.IsNullOrEmpty(_.extraValues.CpfCorretor))
                                                                                   .ToList();

            //Info($"{usuariosBotmaker.Count()} usuários na Botmaker.");

            int qtdeAntes = usuariosBotmaker.Count;
            //BotmakerApi.FiltrarUsuarios(ref usuariosBotmaker, emails: emails, cpfs: cpfs, nomes: nomes);

            //if (qtdeAntes > usuariosBotmaker.Count)
            //    Info($"Usuários no Botmaker filtrados para {usuariosBotmaker.Count}.");

            return usuariosBotmaker;
        }
    }
}