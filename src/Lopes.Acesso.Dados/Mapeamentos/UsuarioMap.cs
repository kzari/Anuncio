using Lopes.Acesso.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lopes.Acesso.Dados.Mapeamentos
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToView("VW_UsuariosAcesso");
            builder.HasKey(_ => _.Id);
        }
    }
}
