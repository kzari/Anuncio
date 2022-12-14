namespace Lopes.Anuncio.Application.Models
{
    public class AnunciosDesatualizadosViewModel
    {
        public AnunciosDesatualizadosViewModel(int portal, FranquiaAnunciosDesatualizados franquiaAnunciosDesatualizados = null)
        {
            Portal = portal;
            if(franquiaAnunciosDesatualizados != null)
                AnunciosDesatualizados =  new List<FranquiaAnunciosDesatualizados> { franquiaAnunciosDesatualizados };
        }

        public int Portal { get; set; }
        public IList<FranquiaAnunciosDesatualizados> AnunciosDesatualizados { get; set; } = new List<FranquiaAnunciosDesatualizados>();

        public class FranquiaAnunciosDesatualizados
        {
            public FranquiaAnunciosDesatualizados(int idFranquia, int idCota)
            {
                IdFranquia = idFranquia;
                IdCota = idCota;
            }

            public int IdCota { get; set; }
            public int IdFranquia { get; set; }
            public int QtdeAnunciosParaExcluir { get; set; }
            public int QtdeAnunciosParaIncluirAtualizar { get; set; }
            public int QtdeAnunciosDesatualizados => QtdeAnunciosParaExcluir + QtdeAnunciosParaIncluirAtualizar;
        }
    }
}
