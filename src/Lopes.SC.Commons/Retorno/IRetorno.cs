namespace Lopes.SC.Commons
{
    public interface IRetorno
    {
        IList<Mensagem> Mensagens { get; }
        bool Sucesso { get; }

        void AdicionarErro(string erro);
        void AdicionarInformacao(string infrormacao);

        string ErrosConcatenados(string separador = ",");
    }
    public interface IRetorno<T> : IRetorno
    {
        T Dado { get; }

        IRetorno<T> AdicionarDado(T caminhoArquivo);
    }
}