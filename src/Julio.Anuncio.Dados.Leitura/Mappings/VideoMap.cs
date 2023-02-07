using Julio.Anuncio.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Julio.Anuncio.Dados.Leitura.Mappings
{
    public class VideoMap : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("tb_UNVI_unidade_video");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id).HasColumnName("UNVI_cd_unidade_video");
            builder.Property(_ => _.Url).HasColumnName("UNVI_nm_url_video");
            builder.Property(_ => _.IdProduto).HasColumnName("UNPR_cd_unidade_pronta");
        }
    }
}