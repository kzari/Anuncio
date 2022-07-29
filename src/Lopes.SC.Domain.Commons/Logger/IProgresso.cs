namespace Lopes.SC.Domain.Commons
{
    public interface IProgresso
    {
        int ValorMaximo { get; }
        void Atualizar(string item, int valorAtual);
        void Atualizar(string item, decimal percentualConcluido);
        void Atualizar(string item);
    }
}
