using Lopes.Anuncio.Domain.ObjetosValor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lopes.Infra.Data.Mappings
{
    public class EmpresaApelidoPortalMap : IEntityTypeConfiguration<EmpresaApelido>
    {
        public void Configure(EntityTypeBuilder<EmpresaApelido> builder)
        {
            builder.ToTable("tb_EMCP_empresa_carga_portal");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).HasColumnName("EMCP_cd_empresa_carga_portal");
            builder.Property(_ => _.IdEmpresa).HasColumnName("EMPE_cd_empresalopes");
            builder.Property(_ => _.Apelido).HasColumnName("EMCP_nickname");
        }
    }
}