namespace Lopes.SC.ExportacaoAnuncio.Application.Services.XML
{
    public class ProcurarIdImoveisVivaRealEnStrategy : ProcurarIdImoveisVivaRealStrategy
    {
        protected override string Query => "ns:ListingDataFeed/ns:Listings/ns:Listing/ns:ListingID";

    }
}
