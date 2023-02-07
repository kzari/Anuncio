using Julio.Acesso.App.Models;
using System.Text;

namespace Julio.Acesso.App.Services
{
    public class UsuarioAcessoAppService : IUsuarioAcessoAppService
    {
        private readonly IUsuarioDadosService _usuarioDadosService;
        private readonly ITokenService _tokenService;

        public UsuarioAcessoAppService(IUsuarioDadosService usuarioDadosService, ITokenService tokenService)
        {
            _usuarioDadosService = usuarioDadosService;
            _tokenService = tokenService;
        }

        public LoginModel ObterTokenAutenticado(string login, string senha)
        {
            Usuario usuario = _usuarioDadosService.ObterUsuario(login);
            if (usuario == null || !VerificarSenha(senha, usuario))
            {
                return new LoginModel("Usuário ou senha incorreto.");
            }

            string token = _tokenService.Gerar(usuario);

            return new LoginModel(usuario, token);
        }



        private static bool VerificarSenha(string senha, Usuario usuario)
        {
            byte[]? plain = Convert.FromBase64String(usuario.Senha);
            Encoding? iso = Encoding.GetEncoding("ISO-8859-6");
            string senhaBase = iso.GetString(plain);
            return senhaBase.Equals(senha);
        }
    }
}