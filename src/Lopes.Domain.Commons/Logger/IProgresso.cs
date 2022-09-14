namespace Lopes.Domain.Commons
{
    public interface IProgresso
    {
        //int ValorMaximo { get; }
        void Mensagem(string texto, decimal percentualConcluido);
        void Atualizar(string texto);
    }
}
