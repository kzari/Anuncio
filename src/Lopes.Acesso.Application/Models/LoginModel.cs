using Lopes.Acesso.App.Models;

namespace Lopes.Acesso.App.Models
{
    public class LoginModel
    {
        public LoginModel(string erro)
        {
            Erro = erro;
        }

        public LoginModel(Usuario usuario, string token)
        {
            Token = token;
            Usuario = new UsuarioModel(usuario);
        }

        public string Token { get; set; }
        public string Erro { get; set; }
        public UsuarioModel Usuario { get; set; }
    }

    public class UsuarioModel
    {
        public UsuarioModel(Usuario usuario)
        {
            Id = usuario.Id;
            Nome = usuario.Nome;
            Login = usuario.Login;
            IdGrupo = usuario.IdGrupo;
            Grupo = usuario.Grupo;
            IdFranquias = usuario.IdFranquias;
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public int IdGrupo { get; set; }
        public string Grupo { get; set; }
        public int[] IdFranquias { get; set; }
    }
}
