namespace Lopes.Botmaker.Application.Models
{
    public class UsuarioIntegracaoBotmakerDTO
    {
        public int CdPessoa { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Apelido { get; set; }
        public string Email { get; set; }
        public int CdEmpresa { get; set; }
        public string? EmailSupervisor { get; set; }
        public string? EmailDiretor { get; set; }
        public string? NomeCargo { get; set; }
        public string? NomeSupervisor { get; set; }
    }
}
