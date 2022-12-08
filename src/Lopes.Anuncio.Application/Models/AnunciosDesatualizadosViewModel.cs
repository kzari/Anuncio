namespace Lopes.Anuncio.Application.Models
{
    public class AnunciosDesatualizadosViewModel
    {
        public AnunciosDesatualizadosViewModel(int portal)
        {
            Portal = portal;
        }

        public int Portal { get; set; }
        public IList<FranquiaAnunciosDesatualizados> AnunciosDesatualizados { get; set; } = new List<FranquiaAnunciosDesatualizados>();

        public class FranquiaAnunciosDesatualizados
        {
            public FranquiaAnunciosDesatualizados(int idFranquia)
            {
                IdFranquia = idFranquia;
            }

            public int IdFranquia { get; set; }
            public int QtdeAnunciosParaExcluir { get; set; }
            public int QtdeAnunciosParaIncluirAtualizar { get; set; }
            public int QtdeAnunciosDesatualizados => QtdeAnunciosParaExcluir + QtdeAnunciosParaIncluirAtualizar;
        }
    }
}
