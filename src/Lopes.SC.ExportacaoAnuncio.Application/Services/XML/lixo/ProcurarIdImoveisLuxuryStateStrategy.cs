namespace Lopes.SC.ExportacaoAnuncio.Application.Services.XML
{
    public class ProcurarIdImoveisLuxuryStateStrategy : ProcurarIdImoveisNamespaceStrategy
    {
        protected override string Query => "ns:publish/ns:properties/ns:property/ns:common_data/ns:reference_code";

        protected override string Namespace => "http://tempuri.org/XMLSchema1.xsd";
    }
}
