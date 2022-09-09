namespace Lopes.Domain.Commons
{
    public interface ILogger
    {
        void Error(string message);
        void Info(string message);
        void Warn(string message);
        void Debug(string message);
        IProgresso ObterProgresso(int valorMaximo, int tamanhoTexto, string textoInicial = null, string caractereSubstituicao = "[contador]");
    }
}
