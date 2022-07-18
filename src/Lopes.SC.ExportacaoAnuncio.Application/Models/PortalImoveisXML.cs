using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Application.Models
{
    public class PortalImoveisXML
    {
        /// <summary>
        /// Imóveis nos XMLs por Portal/Empresa
        /// </summary>
        /// <param name="portal"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="imoveis"></param>
        /// <param name="caminhoArquivo"></param>
        public PortalImoveisXML(Portal portal, int idEmpresa, IEnumerable<int> imoveis, string caminhoArquivo)
        {
            Imoveis = imoveis;
            Portal = portal;
            IdEmpresa = idEmpresa;
            CaminhoArquivo = caminhoArquivo;
        }

        public IEnumerable<int> Imoveis { get; set; }
        public Portal Portal { get; set; }
        public int IdEmpresa { get; set; }
        public string CaminhoArquivo { get; set; }
    }
}