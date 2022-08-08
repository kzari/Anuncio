using Hangfire.Console;
using Hangfire.Console.Progress;
using Hangfire.Server;
using Lopes.SC.Domain.Commons;

namespace Lopes.SC.Jobs.Api.Log
{
    public class HangFireLog : Domain.Commons.ILogger
    {
        private readonly PerformContext _performContext;
        public HangFireLog(PerformContext performContext = null)
        {
            _performContext = performContext;
        }

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
            _performContext.WriteLine(" -- "+mensagem);
            Console.WriteLine(mensagem);
            //Console.ForegroundColor = corLetra;
            //Console.WriteLine(mensagem, ConsoleColor.Green);
            //Console.ForegroundColor = ConsoleColor.White;
        }

        public IProgresso ObterProgresso(int valorMaximo, int tamanhoTexto, string textoInicial = null, string caractereSubstituicao = "[contador]")
        {
            //IProgressBar? progressBar = _performContext.WriteProgressBar();
            //return new Progresso(progressBar);
            return new Progresso2(_performContext, textoInicial, caractereSubstituicao);
        }
    }
}
