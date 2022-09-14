using Lopes.Anuncio.Domain.ObjetosValor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lopes.Infra.Data.Mappings
{
    public class AnuncioMap : IEntityTypeConfiguration<AnuncioCota>
    {
        public void Configure(EntityTypeBuilder<AnuncioCota> builder)
        {
            builder.ToView("VW_ImovelPortais");
            builder.HasKey(_ => _.IdAnuncio);
            //builder.Property(_ => _.)
        }
    }
}