﻿namespace Lopes.SC.ExportacaoAnuncio.Domain.Imovel
{
    public class DadosImovel
    {
        public DadosImovel(DadosPrincipais principais)
        {
            Dados = principais;
        }

        public string? CodigoClientePortal { get; set; }
        public DadosPrincipais Dados { get; set; }
        public IEnumerable<Caracteristica> Caracteristicas { get; set; }
        public IEnumerable<string> UrlTourVirtuais { get; set; }
        public IEnumerable<string> UrlVideos { get; set; }
    }
}
