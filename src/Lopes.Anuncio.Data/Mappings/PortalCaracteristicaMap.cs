using Lopes.Anuncio.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lopes.Infra.Data.Mappings
{
    public class PortalCaracteristicaMap : IEntityTypeConfiguration<PortalCaracteristica>
    {
        public void Configure(EntityTypeBuilder<PortalCaracteristica> builder)
        {
            builder.ToTable("tb_VEIC_x_ATRI_veiculo_atributo");
            builder.HasNoKey();
            builder.Property(_ => _.Portal).HasColumnName("VEIC_cd_veiculo");
            builder.Property(_ => _.IdCaracteristica).HasColumnName("ATRI_cd_atributo");
            builder.Property(_ => _.Tag).HasColumnName("VEAT_ds_tag");
        }
    }
}