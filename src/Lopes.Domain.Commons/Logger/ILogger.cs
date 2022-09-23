namespace Lopes.Domain.Commons
{
    public interface ILogger
    {
        void Error(string message);
        void Info(string message);
        void Warn(string message);
        void Debug(string message);
        IProgresso NovoProgresso(int valorMaximo, int tamanhoTexto, string textoInicial = null, string caractereSubstituicao = "[contador]");
    }
}
