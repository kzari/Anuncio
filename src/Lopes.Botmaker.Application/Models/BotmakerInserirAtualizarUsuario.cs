using Newtonsoft.Json;

namespace Lopes.Botmaker.Application.Models
{
    /// <summary>
    /// Classe que modela os dados do usuário para inclusão/atualização na api da Botmaker
    /// </summary>
    public class BotmakerInserirAtualizarUsuario
    {

        public BotmakerInserirAtualizarUsuario(string cpf,
                                               string email,
                                               string nome,
                                               int cdEmpresa,
                                               string emailDiretor,
                                               string emailSupervisor,
                                               string nomeCargo,
                                               string apelido,
                                               string nomeSupervisor)
        {
            CPF = cpf;
            Email = email;
            Name = MontarNome(nome);
            Lang = "pt";
            Queues = cdEmpresa.ToString();
            EmailDiretor = emailDiretor;
            EmailSupervisor = emailSupervisor;
            NomeSupervisor = nomeSupervisor;
            PessoaCargo = ObterCargo(nomeCargo);
            Alias = apelido;
            Groups = GrupoBotmaker(nomeCargo);

            switch (PessoaCargo)
            {
                case Cargo.Outro:
                case Cargo.Corretor:
                    ShowMyCharts = "true";
                    Priority = "NORMAL";
                    Role = "OPERATOR";
                    Slots = 1;
                    break;

                case Cargo.Diretor:
                    ShowMyCharts = "false";
                    Priority = "NEVER";
                    CustomRole = "DIRETOR LOPES";
                    break;

                case Cargo.Supervisor:
                    ShowMyCharts = "false";
                    Priority = "NEVER";
                    CustomRole = "SUPERVISOR LOPES";
                    break;
            }
            ExtraValues = JsonConvert.SerializeObject(new
            {
                Apelido = Alias,
                CpfCorretor = CPF,
                EmailSupervisor,
                top_name = NomeSupervisor,
                EmailDiretor
            });
        }


        public string CPF { get; private set; }
        public string EmailSupervisor { get; private set; }
        public string EmailDiretor { get; private set; }
        public string NomeSupervisor { get; private set; }


        [JsonProperty("email")]
        public string Email { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("lang")]
        public string Lang { get; private set; }

        [JsonProperty("groups")]
        public string Groups { get; private set; }

        [JsonProperty("priority")]
        public string Priority { get; private set; }

        [JsonProperty("queues")]
        public string Queues { get; private set; }

        [JsonProperty("extraValues")]
        public string ExtraValues { get; private set; }

        [JsonProperty("showMyChats")]
        public string ShowMyCharts { get; private set; }

        [JsonProperty("alias")]
        public string Alias { get; private set; }

        [JsonProperty("role")]
        public string Role { get; private set; }

        [JsonProperty("customRole")]
        public string CustomRole { get; private set; }

        [JsonProperty("slots")]
        public int Slots { get; private set; }


        enum Cargo
        {
            Corretor,
            Supervisor,
            Diretor,
            Outro
        }

        private string MontarNome(string nome)
        {
            const int qtdeMaxima = 50;

            if (nome.Length > qtdeMaxima)
                nome = nome.Substring(0, qtdeMaxima);

            return nome;
        }

        private Cargo PessoaCargo { get; set; }

        private Cargo ObterCargo(string cargo)
        {
            if (string.IsNullOrEmpty(cargo))
                return Cargo.Outro;

            return cargo.ToLower().Contains("diretor")
                ? Cargo.Diretor
                : cargo.ToLower().Contains("corretor")
                    ? Cargo.Corretor
                    : cargo.ToLower().Contains("supervisor") || cargo.ToLower().Contains("gerente") || cargo.ToLower().Contains("superintendente")
                        ? Cargo.Supervisor
                        : Cargo.Outro;
        }

        private string GrupoBotmaker(string nomeCargo)
        {
            if (nomeCargo.ToLower() == "diretor geral")
                return "Diretor";

            return nomeCargo;
        }
    }
}
