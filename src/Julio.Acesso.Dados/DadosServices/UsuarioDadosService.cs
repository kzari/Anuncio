﻿using Julio.Acesso.App.Models;
using Julio.Acesso.App.Services;
using Microsoft.EntityFrameworkCore;

namespace Julio.Acesso.Dados.DadosServices
{
    public class UsuarioDadosService : IUsuarioDadosService
    {
        private readonly AcessoDadosContext _db;

        public UsuarioDadosService(AcessoDadosContext context)
        {
            _db = context;
        }

        public Usuario? ObterUsuario(string login)
        {
            IQueryable<Usuario>? query = _db.Set<Usuario>().AsNoTracking().Where(_ => _.Login == login && _.Ativo);
            return query.FirstOrDefault();
        }
    }
}
