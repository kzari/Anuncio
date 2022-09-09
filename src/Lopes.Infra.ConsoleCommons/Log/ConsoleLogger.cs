using Lopes.Domain.Commons;

namespace Lopes.Infra.ConsoleCommons.Log
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            WriteLine(message, ConsoleColor.White);
        }

        public void Warn(string message)
        {
            WriteLine(message, ConsoleColor.Yellow);
        }

        public void Error(string message)
        {
            WriteLine(message, ConsoleColor.Red);
        }

        public void Debug(string message)
        {
#if DEBUG
            WriteLine(message, ConsoleColor.Green);
#endif 
        }


        private void WriteLine(string mensagem, ConsoleColor corLetra)
        {
            Console.ForegroundColor = corLetra;
            Console.WriteLine(mensagem, ConsoleColor.Green);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public IProgresso ObterProgresso(int valorMaximo, int tamanhoTexto, string textoInicial = null, string caractereSubstituicao = "[contador]")
        {
            return new Progresso(valorMaximo, tamanhoTexto, textoInicial, caractereSubstituicao);
        }
    }
}
