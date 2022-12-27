namespace Lopes.Domain.Commons
{
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
    }

    public class Retorno<T> : Resultado, IResultado<T>
    {
        public Retorno()
        {
        }

        public Retorno(T dado)
        {
            Dado = dado;
        }

        public T Dado { get; private set; }


        public IResultado<T> AdicionarDado(T caminhoArquivo)
        {
            Dado = caminhoArquivo;
            return this;
        }
    }
}