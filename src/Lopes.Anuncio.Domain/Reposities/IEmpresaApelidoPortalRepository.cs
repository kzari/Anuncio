using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Domain.Reposities
{
    public interface IEmpresaApelidoPortalRepository
    {
        IEnumerable<EmpresaApelido> Obter();
    }
}
