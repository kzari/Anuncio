namespace Julio.Domain.Commons
{
    public interface IResultado
    {
        IList<Mensagem> Mensagens { get; }
        bool Sucesso { get; }
        bool Falha => !Sucesso;

        IResultado AdicionarErro(string erro);
        IResultado AdicionarInformacao(string infrormacao);

        string ErrosConcatenados(string separador = ",");
    }
    public interface IResultado<T> : IResultado
    {
        T Dado { get; }

        IResultado<T> AdicionarDado(T caminhoArquivo);
    }
}