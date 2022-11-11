using Lopes.Anuncio.Domain.ObjetosValor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lopes.Anuncio.Dados.Leitura.Mappings
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