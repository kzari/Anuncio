using Lopes.Domain.Commons;

namespace Lopes.Jobs.Web.Log
{
    public class Log : Domain.Commons.ILogger
    {
        public void Debug(string message)
        {
        }

        public void Error(string message)
        {
        }

        public void Info(string message)
        {
        }

        public IProgresso ObterProgresso(int valorMaximo, int tamanhoTexto, string textoInicial = null, string caractereSubstituicao = "[contador]")
        {
            return null;
        }

        public void Warn(string message)
        {
        }
    }
}
