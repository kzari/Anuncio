using Lopes.SC.Domain.Commons;

namespace Lopes.SC.Infra.Commons
{
    public class Retorno : IRetorno
    {
        public Retorno() => Mensagens = new List<Mensagem>();
        public Retorno(params Mensagem[] mensagens) => Mensagens = mensagens.ToList();
        public Retorno(IEnumerable<Mensagem> mensagens) => Mensagens = mensagens.ToList();

        public IList<Mensagem> Mensagens { get; }
        public bool Sucesso => !Mensagens.Any(_ => _.Tipo == TipoMensagem.Erro);

        public IRetorno AdicionarErro(string erro)
        {
            Mensagens.Add(new Mensagem(TipoMensagem.Erro, erro));
            return this;
        }
        public IRetorno AdicionarInformacao(string informacao)
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

    public class Retorno<T> : Retorno, IRetorno<T>
    {
        public Retorno()
        {
        }

        public Retorno(T dado)
        {
            Dado = dado;
        }

        public T Dado { get; private set; }


        public IRetorno<T> AdicionarDado(T caminhoArquivo)
        {
            Dado = caminhoArquivo;
            return this;
        }
    }
}