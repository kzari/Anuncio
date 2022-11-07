namespace Lopes.Acesso.Application.Services
{
    public interface IUsuarioAcessoAppService
    {
        string ObterTokenAutenticado(string login, string senha);
    }
}