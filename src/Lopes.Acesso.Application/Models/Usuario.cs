namespace Lopes.Acesso.App.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int IdGrupo { get; set; }
        public string Grupo { get; set; }
        public string Senha { get; set; }
        public int IdPessoa { get; set; }
        public DateTime? DataAlteracaoSenha { get; set; }
        public DateTime? DataAlteracaoCadastro { get; set; }
        public int RT { get; set; }
        public int IdUsuarioInclusao { get; set; }
        public DateTime DtInclusao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DtExpiracao { get; set; }
        public bool ExcecaoAD { get; set; }
        public string IdFranquiasExtenso { get; set; }
        public int[] IdFranquias => string.IsNullOrEmpty(IdFranquiasExtenso)
            ? Array.Empty<int>()
            : IdFranquiasExtenso.Split(",").Where(_ => !string.IsNullOrEmpty(_))
                                           .Select(_ => int.Parse(_))
                                           .ToArray();

    }
}
