using Lopes.Anuncio.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lopes.Infra.Data.Mappings
{
    public class AnuncioMap : IEntityTypeConfiguration<Anuncio.Domain.Models.AnuncioImovel>
    {
        public void Configure(EntityTypeBuilder<Anuncio.Domain.Models.AnuncioImovel> builder)
        {
            builder.ToView("VW_ImovelPortais");
            builder.HasKey(_ => _.IdAnuncio);
            //builder.Property(_ => _.)
        }
    }
}