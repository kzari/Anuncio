﻿using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Models.XML;

namespace Lopes.SC.ExportacaoAnuncio.Domain.XML
{
    public interface IPortalXMLElementos
    {
        Xml ObterXml(IEnumerable<DadosImovel> imoveis);
        Xml ObterXml(DadosImovel dados);
    }
}