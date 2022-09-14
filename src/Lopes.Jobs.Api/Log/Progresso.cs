using Hangfire.Console;
using Hangfire.Console.Progress;
using Hangfire.Server;
using Lopes.Domain.Commons;

namespace Lopes.Jobs.Api.Log
{
    public class Progresso : IProgressBar, IProgresso
    {
        private readonly IProgressBar _progressBar;

        public Progresso(IProgressBar progressBar)
        {
            _progressBar = progressBar;
        }

        public int ValorMaximo { get; }

        public void Atualizar(string item)
        {
            //progressBar.Next(texto);
        }

        public void Mensagem(string item, decimal percentualConcluido)
        {
            if (percentualConcluido > 100)
                percentualConcluido = 100;
            //int qtde = ObterQuantidadePorPercentual(percentualConcluido);
            _progressBar.SetValue((int)percentualConcluido);
        }

        public void SetValue(int value) => _progressBar.SetValue(value);
        public void SetValue(double value) => _progressBar.SetValue(value);
    }



    public class Progresso2 : IProgressBar, IProgresso
    {
        private readonly PerformContext _performContext;
        private readonly string _textoInicial;
        private readonly string _caractereSubstituicao;

        public Progresso2(PerformContext performContext, string textoInicial, string caractereSubstituicao)
        {
            _performContext = performContext;
            _textoInicial = textoInicial;
            _caractereSubstituicao = caractereSubstituicao;
        }

        public int ValorMaximo { get; }

        public void Atualizar(string item)
        {
            string texto = ObterTexto(item);
            _performContext.WriteLine(texto);
            Console.WriteLine(item);
        }

        public void Mensagem(string item, decimal percentualConcluido)
        {
            string texto = ObterTexto(item);
            _performContext.WriteLine(texto);
            Console.WriteLine(item);
        }

        public void SetValue(int value) { }
        public void SetValue(double value) { }



        private string ObterTexto(string item, int? valorAtual = null)
        {
            string texto = string.IsNullOrEmpty(_textoInicial)
                            ? item
                            : _textoInicial + " >> " + item;

            if (valorAtual.HasValue && !string.IsNullOrEmpty(texto))
                texto = texto.Replace(_caractereSubstituicao, valorAtual.ToString());

            return " -- " + texto;
        }
    }
}
