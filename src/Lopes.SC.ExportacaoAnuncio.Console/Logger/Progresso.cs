using Konsole;
using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;

namespace Lopes.SC.ExportacaoAnuncio.ConsoleTestes
{
    public class Progresso : ProgressBar, IProgresso
    {
        private readonly IProgressBar progressBar;

        public Progresso(int valorMaximo, int tamanhoTexto) : base(valorMaximo, tamanhoTexto)
        {
            ValorMaximo = valorMaximo;
            progressBar = new ProgressBar(valorMaximo, tamanhoTexto);
        }

        public int ValorMaximo { get; }

        public void Atualizar(int valorAtual, string item)
        {
            progressBar.Refresh(valorAtual, item);
        }

        public void Atualizar(string item)
        {
            progressBar.Next(item);
        }
    }
}
