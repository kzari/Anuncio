namespace Lopes.Botmaker.Application.Models
{
    public class UsuarioBotmakerApi
    {
        public DateTime? lastModificationDate { get; set; }
        public ExtraValues extraValues { get; set; }
        public string name { get; set; }
        public string email { get; set; }

        public bool UsuarioAdmin => extraValues == null || string.IsNullOrEmpty(extraValues.CpfCorretor);
    }

    public class ExtraValues
    {
        public string Apelido { get; set; }
        public string CpfCorretor { get; set; }
        public string EmailSupervisor { get; set; }
        public string top_name { get; set; }
        public string EmailDiretor { get; set; }
    }

    public class ListaUsuariosBotmaker
    {
        public List<UsuarioBotmakerApi> active { get; set; }
    }

    public class Token
    {
        public string token { get; set; }
    }
}
