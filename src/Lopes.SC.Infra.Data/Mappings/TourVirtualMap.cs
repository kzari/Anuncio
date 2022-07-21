﻿using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lopes.SC.Infra.Data.Mappings
{
    public class TourVirtualMap : IEntityTypeConfiguration<TourVirtual>
    {
        public void Configure(EntityTypeBuilder<TourVirtual> builder)
        {
            builder.ToTable("tb_UNTV_unidade_tour_virtual");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id).HasColumnName("UNTV_cd_unidade_tour_virtual");
            builder.Property(_ => _.Url).HasColumnName("UNTV_nm_url_tour_virtual");
            builder.Property(_ => _.IdImovel).HasColumnName("UNPR_cd_unidade_pronta");
        }
    }
}