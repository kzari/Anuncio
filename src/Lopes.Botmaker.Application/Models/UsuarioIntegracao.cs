namespace Lopes.Botmaker.Application.Models
{
    public class UsuarioIntegracao
    {
        private List<string> _alteracoes;

        public UsuarioIntegracao(DadosUsuarioDTO usuario, UsuarioBotmakerApi? usuarioBotmaker = null)
        {
            UsuarioSistema = usuario;
            UsuarioBotmaker = usuarioBotmaker;
        }
        public UsuarioIntegracao(UsuarioBotmakerApi usuarioBotmaker)
        {
            UsuarioBotmaker = usuarioBotmaker;
        }

        public DadosUsuarioDTO? UsuarioSistema { get; set; }
        public UsuarioBotmakerApi? UsuarioBotmaker { get; set; }

        public string Cpf => UsuarioSistema?.CPF ?? UsuarioBotmaker.extraValues.CpfCorretor;
        public string Apelido => UsuarioSistema?.Apelido ?? UsuarioBotmaker.extraValues.Apelido;
        public string Nome => UsuarioSistema?.Nome ?? UsuarioBotmaker.name;
        public string Email => UsuarioSistema?.Email ?? UsuarioBotmaker.email;
        public string? EmailSupervisor => UsuarioSistema?.EmailSupervisor ?? UsuarioBotmaker?.extraValues?.EmailSupervisor;
        public string? EmailDiretor => UsuarioSistema?.EmailDiretor ?? UsuarioBotmaker?.extraValues?.EmailDiretor;
        public string? NomeSupervisor => UsuarioSistema?.NomeSupervisor ?? UsuarioBotmaker?.extraValues?.top_name;

        public bool Novo => UsuarioSistema != null && UsuarioBotmaker == null;
        public bool Excluir => UsuarioBotmaker != null && UsuarioSistema == null;
        public bool Atualizar => ObterAlteracoes().Any();
        public string Acao => Novo ? "Inserir" : Excluir ? "Excluir" : Atualizar ? "Atualizar" : "Atualizado";



        /// <summary>
        /// Retorna as diferenças entre o usuário da Botmaker e do usuário para integrar
        /// </summary>
        /// <returns></returns>
        private string[] ObterAlteracoes()
        {
            if (UsuarioSistema == null || UsuarioBotmaker == null)
                return Array.Empty<string>();

            if (_alteracoes != null)
                return _alteracoes.ToArray();

            _alteracoes = new List<string>();

            if (UsuarioSistema.NomeSupervisor?.ToLower() != UsuarioBotmaker.extraValues?.top_name?.ToLower())
                _alteracoes.Add($"Nome do Supervisor alterado de '{UsuarioBotmaker.extraValues?.top_name?.ToLower() ?? ""}' para '{UsuarioSistema.NomeSupervisor ?? ""}'");

            if (UsuarioSistema.Email.ToLower() != UsuarioBotmaker.email.ToLower())
                _alteracoes.Add($"E-mail alterado de '{UsuarioBotmaker.email.ToLower()}' para '{UsuarioSistema.Email}'");

            if (UsuarioSistema.Nome?.ToUpper() != UsuarioBotmaker.name?.ToUpper())
                _alteracoes.Add($"Nome alterado de '{UsuarioBotmaker.name ?? ""}' para '{UsuarioSistema.Nome ?? ""}'");

            if (UsuarioSistema.Apelido?.ToUpper() != UsuarioBotmaker.extraValues?.Apelido?.ToUpper())
                _alteracoes.Add($"Nome alterado de '{UsuarioBotmaker.extraValues?.Apelido ?? ""}' para '{UsuarioSistema.Apelido ?? ""}'");

            if (UsuarioSistema.EmailDiretor?.ToUpper() != UsuarioBotmaker.extraValues?.EmailDiretor?.ToUpper())
                _alteracoes.Add($"E-mail do diretor alterado de '{UsuarioBotmaker.extraValues?.EmailDiretor ?? ""}' para '{UsuarioSistema.EmailDiretor ?? ""}'");

            if (UsuarioSistema.EmailSupervisor?.ToUpper() != UsuarioBotmaker.extraValues?.EmailSupervisor?.ToUpper())
                _alteracoes.Add($"E-mail do Supervisor alterado de '{UsuarioBotmaker.extraValues?.EmailSupervisor ?? ""}' para '{UsuarioSistema.EmailSupervisor ?? ""}'");

            return _alteracoes.ToArray();
        }
    }
}
