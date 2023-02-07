using Julio.Anuncio.Domain.ObjetosValor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Julio.Anuncio.Dados.Leitura.Mappings
{
    public class AnuncioMap : IEntityTypeConfiguration<AnuncioCota>
    {
        public void Configure(EntityTypeBuilder<AnuncioCota> builder)
        {
            builder.ToView("VW_Anuncios");
            builder.HasKey(_ => _.IdAnuncio);
        }
    }
}