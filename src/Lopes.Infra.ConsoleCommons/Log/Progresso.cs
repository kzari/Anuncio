using Konsole;
using Lopes.Domain.Commons;

namespace Lopes.Infra.ConsoleCommons.Log
{
    public class Progresso : ProgressBar, IProgresso
    {
        private readonly IProgressBar progressBar;
        private readonly string _textoInicial;
        private readonly string _caractereSubstituicao;

        public Progresso(int valorMaximo, int tamanhoTexto, string textoInicial = null, string caractereSubstituicao = "[contador]") : base(valorMaximo, tamanhoTexto)
        {
            ValorMaximo = valorMaximo;
            progressBar = new ProgressBar(valorMaximo, tamanhoTexto);
            _textoInicial = textoInicial;
            _caractereSubstituicao = caractereSubstituicao;
        }

        public int ValorMaximo { get; }

        public void NovaMensagem(string item, int valorAtual)
        {
            string texto = ObterTexto(item, valorAtual);

            if (valorAtual > 0)
            {
                try
                {
                    progressBar.Refresh(valorAtual, texto);
                }
                catch (Exception ex){}
            }
        }
        public void NovaMensagem(string item)
        {
            string texto = ObterTexto(item);
            try
            {
                progressBar.Next(texto);
            }
            catch (Exception ex){}
        }

        public void Mensagem(string item, decimal percentualConcluido)
        {
            int qtde = ObterQuantidadePorPercentual(percentualConcluido);
            NovaMensagem(item, qtde);
        }

        private int ObterQuantidadePorPercentual(decimal percentual)
        {
            return (int)((percentual/100) * ValorMaximo);
        }

        private string ObterTexto(string item, int? valorAtual = null)
        {
            string texto = string.IsNullOrEmpty(_textoInicial)
                            ? item
                            : _textoInicial + " >> " + item;

            if (valorAtual.HasValue && !string.IsNullOrEmpty(texto))
                texto = texto.Replace(_caractereSubstituicao, valorAtual.ToString());

            return texto;
        }
    }
}
