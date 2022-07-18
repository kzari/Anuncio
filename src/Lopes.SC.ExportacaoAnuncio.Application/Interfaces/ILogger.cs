namespace Lopes.SC.ExportacaoAnuncio.Application.Interfaces
{
    public interface ILogger
    {
        void Error(string message);
        void Info(string message);
        void Warn(string message);
        void Debug(string message);
    }
}
