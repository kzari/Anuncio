﻿using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Interfaces
{
    public interface IEmpresaApelidoPortalRepository
    {
        IEnumerable<EmpresaApelidoPortal> Obter();
    }
}
