namespace Lopes.SC.ExportacaoAnuncio.Domain.Models
{
    public class EmpresaApelidoPortal
    {
        public int Id { get; set; }
        public int IdEmpresa { get; private set; }
        public string Apelido { get; private set; }
    }
}
