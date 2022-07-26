namespace Lopes.SC.ExportacaoAnuncio.Application.Interfaces
{
    public interface IProgresso
    {
        int ValorMaximo { get; }
        void Atualizar(int valorAtual, string item);
        void Atualizar(string item);
    }
}
