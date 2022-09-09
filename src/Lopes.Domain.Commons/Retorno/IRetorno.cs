namespace Lopes.Domain.Commons
{
    public interface IRetorno
    {
        IList<Mensagem> Mensagens { get; }
        bool Sucesso { get; }

        IRetorno AdicionarErro(string erro);
        IRetorno AdicionarInformacao(string infrormacao);

        string ErrosConcatenados(string separador = ",");
    }
    public interface IRetorno<T> : IRetorno
    {
        T Dado { get; }

        IRetorno<T> AdicionarDado(T caminhoArquivo);
    }
}