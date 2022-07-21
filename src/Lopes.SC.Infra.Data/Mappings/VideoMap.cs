using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lopes.SC.Infra.Data.Mappings
{
    public class VideoMap : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("tb_UNVI_unidade_video");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id).HasColumnName("UNVI_cd_unidade_video");
            builder.Property(_ => _.Url).HasColumnName("UNVI_nm_url_video");
            builder.Property(_ => _.IdImovel).HasColumnName("UNPR_cd_unidade_pronta");
        }
    }
}