using Julio.Domain.Commons;

namespace Julio.Jobs.Web.Log
{
    public class Log : Domain.Commons.ILogger
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
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
