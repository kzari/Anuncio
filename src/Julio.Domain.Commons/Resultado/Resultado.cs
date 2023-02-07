namespace Julio.Domain.Commons
{
    public class Falha<T> : Resultado<T>
    {
        public Falha(string erro) : base()
        {
            AdicionarErro(erro);
        }
    }
    public class Falha : Resultado
    {
        public Falha(string erro) :base(new Mensagem(TipoMensagem.Erro, erro))
        {
        }
    }

    public class Sucesso<T> : Resultado<T>
    {
        public Sucesso(T dado) : base(dado)
        {
        }
    }
    public class Sucesso : Resultado
    {
    }

    public class Resultado : IResultado
    {
        public Resultado() => Mensagens = new List<Mensagem>();
        public Resultado(params Mensagem[] mensagens) => Mensagens = mensagens.ToList();
        public Resultado(IEnumerable<Mensagem> mensagens) => Mensagens = mensagens.ToList();

        public IList<Mensagem> Mensagens { get; }
        public bool Sucesso => !Mensagens.Any(_ => _.Tipo == TipoMensagem.Erro);


        public IResultado AdicionarErro(string erro)
        {
            Mensagens.Add(new Mensagem(TipoMensagem.Erro, erro));
            return this;
        }
        public IResultado AdicionarInformacao(string informacao)
        {
            Mensagens.Add(new Mensagem(TipoMensagem.Informacao, informacao));
            return this;
        }

        public string ErrosConcatenados(string separador = ",")
        {
            string[] erros = Mensagens.Where(_ => _.Tipo == TipoMensagem.Erro)
                                      .Select(_ => _.Descricao)
                                      .ToArray();
            return erros.Any()
                ? string.Join(separador, erros.ToArray())
                : string.Empty;
        }

        public static Falha ComErro(string erro) => new Falha(erro);
        public static Sucesso ComSucesso => new Sucesso();
    }

    public class Resultado<T> : Resultado, IResultado<T>
    {
        public Resultado()
        {
        }

        public Resultado(T dado)
        {
            Dado = dado;
        }

        public T Dado { get; private set; }


        public IResultado<T> AdicionarDado(T caminhoArquivo)
        {
            Dado = caminhoArquivo;
            return this;
        }
        public new IResultado<T> AdicionarErro(string erro)
        {
            Mensagens.Add(new Mensagem(TipoMensagem.Erro, erro));
            return this;
        }

        public new static Falha<T> ComErro(string mensagem) => new Falha<T>(mensagem);
        public new static Sucesso<T> ComSucesso(T dado) => new Sucesso<T>(dado);
    }
}