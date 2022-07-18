using System;

namespace Lopes.SC.Common
{
    public static class ConsoleExtensions
    {
        public static void Warn(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Yellow; 
            Console.WriteLine(mensagem, ConsoleColor.Gray);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

}