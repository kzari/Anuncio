using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lopes.SC.Infra.Data.Mappings
{
    public class AnuncioMap : IEntityTypeConfiguration<Anuncio>
    {
        public void Configure(EntityTypeBuilder<Anuncio> builder)
        {
            builder.ToView("VW_ImovelPortais");
            builder.HasKey(_ => _.IdAnuncio);
            //builder.Property(_ => _.)
        }
    }
}