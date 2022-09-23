using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Domain.Handlers
{
    public struct AnunciosAgrupados
    {
        public Portal Portal { get; internal set; }
        public int IdFranquia { get; internal set; }
        public int IdCota { get; internal set; }
        public IEnumerable<AnuncioCota> Anuncios { get; internal set; }
    }
}