using Lopes.Anuncio.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lopes.Acesso.Data.Mappings
{
    public class AnuncioAtualizacaoMap : IEntityTypeConfiguration<AnuncioAtualizacao>
    {
        public void Configure(EntityTypeBuilder<AnuncioAtualizacao> builder)
        {
            builder.ToTable("AnuncioAtualizacao").HasKey(_ => _.Id);
        }
    }
}