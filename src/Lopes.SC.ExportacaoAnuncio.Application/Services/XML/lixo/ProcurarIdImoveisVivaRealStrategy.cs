namespace Lopes.SC.ExportacaoAnuncio.Application.Services.XML
{
    public class ProcurarIdImoveisVivaRealStrategy : ProcurarIdImoveisNamespaceStrategy
    {
        protected override string Query => "ns:Document/ns:imoveis/ns:imovel/ns:referencia";

        protected override string Namespace => "http://www.vivareal.com/schemas/1.0/VRSync";
    }
}
