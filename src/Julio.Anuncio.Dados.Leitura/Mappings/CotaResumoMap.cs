using Julio.Anuncio.Domain.ObjetosValor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Julio.Anuncio.Dados.Leitura.Mappings
{
    public class CotaResumoMap : IEntityTypeConfiguration<CotaResumo>
    {
        public void Configure(EntityTypeBuilder<CotaResumo> builder)
        {
            builder.ToView("VW_CotaResumo");
            builder.HasNoKey();
        }
    }
}