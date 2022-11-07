using Lopes.Acesso.Domain.Models;
using Lopes.Acesso.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Acesso.Dados.DadosServices
{
    public class UsuarioDadosService : IUsuarioDadosService
    {
        protected readonly DbLopesnetContext Db;

        public UsuarioDadosService(DbLopesnetContext context)
        {
            Db = context;
        }

        public Usuario? ObterUsuario(string login)
        {
            return Db.Set<Usuario>().Where(_ => _.Login == login)
                                    .AsNoTracking()
                                    .FirstOrDefault();
        }
    }
}
