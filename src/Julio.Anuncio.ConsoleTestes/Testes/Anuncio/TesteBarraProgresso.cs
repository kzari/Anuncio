using Konsole;

namespace Julio.ConsoleTestes.Testes.Anuncio
{
    public static class TesteBarraProgresso
    {
        public static void Barrinha()
        {
            var progresso = new ProgressBar(10);
            progresso.Refresh(1, "A");
            progresso.Refresh(2, "B");
            progresso.Refresh(3, "C");
            progresso.Refresh(4, "C");
            progresso.Refresh(5, "C");
            progresso.Refresh(6, "C");
            progresso.Next("DD");
            progresso.Refresh(8, "C");
        }
    }
}
