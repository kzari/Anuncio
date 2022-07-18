namespace Lopes.SC.ExportacaoAnuncio.Application.Services.XML
{
    public class ProcurarIdImoveisChaveFacilStrategy : ProcurarIdImoveisStrategy
    {
        protected override string Query => "//Importacao/Imoveis/Imovel/Codigo";
    }
}
