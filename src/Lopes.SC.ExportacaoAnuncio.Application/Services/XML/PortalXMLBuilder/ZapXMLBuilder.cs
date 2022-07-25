namespace Lopes.SC.ExportacaoAnuncio.Application.Services.XML
{
    //public class ZapXMLBuilder : PortalXMLBuilder, IPortalXMLBuilder
    //{
    //    public ZapXMLBuilder(Portal portal, string caminhoArquivo) : base(portal, caminhoArquivo)
    //    {
    //    }

    //    public virtual IEnumerable<int> ObterIdImoveisNoXml() => ObterIdImoveisNoXml("//Imoveis/Imovel/CodigoImovel");


    //    public virtual void InserirAtualizarImovel(Imovel dados)
    //    {
    //        if (!File.Exists(CaminhoArquivo))
    //            CriarXml();

    //        XmlDocument doc = new XmlDocument();
    //        doc.Load(CaminhoArquivo);

    //        XmlNode? eImoveis = doc.SelectSingleNode("/Carga/Imoveis");

    //        //Removendo se existir
    //        XmlNode? eImovelExistente = doc.SelectSingleNode(QueryIdImovel(dados.IdImovelNoPortal));
    //        if (eImovelExistente != null)
    //            eImoveis.RemoveChild(eImovelExistente.ParentNode);

    //        //Adicionando
    //        XmlElement? eImovel = doc.CreateElement("Imovel");
    //        eImoveis.AppendChild(eImovel);

    //        DeterminaTipoSubtipoCategoriaDoImovel(dados, out string tipoImovel, out string subTipoImovel, out string categoria);

    //        AdicionarElemento(doc, eImovel, "CodigoImovel", dados.IdImovelNoPortal);

    //        if(!string.IsNullOrEmpty(dados.CodigoClientePortal))
    //            AdicionarElemento(doc, eImovel, "CodigoCliente", dados.CodigoClientePortal);

    //        AdicionarElemento(doc, eImovel, "TipoImovel", tipoImovel);
    //        AdicionarElemento(doc, eImovel, "UF", dados.Estado);
    //        AdicionarElemento(doc, eImovel, "Cidade", dados.Cidade);
    //        AdicionarElemento(doc, eImovel, "Bairro", dados.Bairro);
    //        AdicionarElemento(doc, eImovel, "Endereco", dados.Logradouro);
    //        AdicionarElemento(doc, eImovel, "Numero", dados.Numero);
    //        AdicionarElemento(doc, eImovel, "CEP", dados.CEP);

    //        if (dados.ValorVenda.HasValue)
    //            AdicionarElemento(doc, eImovel, "PrecoVenda", dados.ValorVenda.Value.ToString());

    //        if (dados.ValorLocacao.HasValue)
    //            AdicionarElemento(doc, eImovel, "PrecoLocacao", dados.ValorLocacao.Value.ToString());

    //        if(dados.ValorCondominio.HasValue)
    //            AdicionarElemento(doc, eImovel, "PrecoCondominio", dados.ValorCondominio.Value.ToString());

    //        if (dados.ValorIPTU.HasValue)
    //            AdicionarElemento(doc, eImovel, "ValorIPTU", dados.ValorIPTU.Value.ToString());


    //        AdicionarElemento(doc, eImovel, "QtdSuites", dados.QtdeSuites.ToString());
    //        AdicionarElemento(doc, eImovel, "QtdBanheiros", dados.QtdeBanheirosSociais.ToString());
    //        AdicionarElemento(doc, eImovel, "QtdVagas", dados.QtdeVagas.ToString());

    //        if(dados.AnoConstrucao.HasValue)
    //        AdicionarElemento(doc, eImovel, "AnoConstrucao", dados.AnoConstrucao.ToString());

    //        if(dados.AreaUtil.HasValue)
    //            AdicionarElemento(doc, eImovel, "AreaUtil", dados.AreaUtil.ToString());

    //        if(dados.AreaTotal.HasValue)
    //            AdicionarElemento(doc, eImovel, "AreaTotal", dados.AreaTotal.ToString());

    //        AdicionarElemento(doc, eImovel, "QtdDormitorios", Terreno(tipoImovel) ? "0" : dados.QtdeQuartos == 0 ? "1" : dados.QtdeQuartos.ToString());

    //        string observacao = SubstituirCaracteresInvalidosUnicode(dados.TextoSite) + " - Ref.: " + dados.IdImovel;
    //        AdicionarElemento(doc, eImovel, "Observacao", observacao);

    //        AdicionarTourVirtual(doc, eImoveis, dados);
    //        AdicionarVideos(doc, eImoveis, dados);

    //        //TODO: adicionar imagens

    //        doc.Save(CaminhoArquivo);
    //    }

    //    public virtual void RemoverImovel(int idImovel)
    //    {
    //        if (!File.Exists(CaminhoArquivo))
    //            return;

    //        XmlDocument doc = new XmlDocument();
    //        doc.Load(CaminhoArquivo);

    //        XmlNode? eImoveis = doc.SelectSingleNode("/Carga/Imoveis");

    //        //Removendo se existir
    //        XmlNode? eImovelExistente = doc.SelectSingleNode(QueryIdImovel(idImovel));
    //        if (eImovelExistente != null)
    //            eImoveis.RemoveChild(eImovelExistente.ParentNode);

    //        doc.Save(CaminhoArquivo);
    //    }

    //    private static void AdicionarTourVirtual(XmlDocument doc, XmlNode eImoveis, Imovel imovel)
    //    {
    //        if (imovel.UrlTourVirtuais == null || !imovel.UrlTourVirtuais.Any())
    //            return;

    //        foreach (var url in imovel.UrlTourVirtuais)
    //        {
    //            XmlElement elemento = doc.CreateElement("LinkTourVirtual");
    //            elemento.InnerText = url;
    //            eImoveis.AppendChild(elemento);
    //        }
    //    }

    //    private static void AdicionarVideos(XmlDocument doc, XmlNode eImoveis, Imovel imovel)
    //    {
    //        if (imovel.UrlVideos == null || !imovel.UrlVideos.Any())
    //            return;

    //        XmlElement eVideos = doc.CreateElement("Videos");
    //        eImoveis.AppendChild(eVideos);

    //        foreach (var url in imovel.UrlVideos)
    //        {
    //            XmlElement elemento = doc.CreateElement("Video");
    //            elemento.InnerText = url;
    //            eVideos.AppendChild(elemento);
    //        }
    //    }

    //    private static bool Terreno(string descrition) => descrition.Contains("Terreno");

    //    private void DeterminaTipoSubtipoCategoriaDoImovel(Imovel imovel, out string tipo, out string subtipo, out string categoria)
    //    {
    //        if (imovel.Subtipo == "Estúdio" || imovel.Subtipo == "Studio")//Alterado porque Zap não tem studio abaixo de apartamento
    //        {
    //            imovel.Tipo = "Comercial";
    //        }

    //        switch (imovel.Tipo.Trim())
    //        {
    //            case "Casa":
    //                tipo = imovel.Tipo.Trim();
    //                subtipo = DeterminaSubTipoDoImovelParaTipoCasa(imovel.Subtipo);
    //                categoria = DeterminaCategoriaParaTipoCasa(imovel.Subtipo);
    //                break;
    //            case "Galpão":
    //                tipo = "Comercial/Industrial";
    //                subtipo = "Galpão";
    //                categoria = "Padrão";
    //                break;
    //            case "Hotel":
    //                tipo = "Comercial/Industrial";
    //                subtipo = DeterminaSubTipoDoImovelParaTipoHotel(imovel.Subtipo);
    //                categoria = "Padrão";
    //                break;
    //            case "Prédio":
    //                tipo = "Comercial/Industrial";
    //                subtipo = "Prédio Inteiro";
    //                categoria = "Padrão";
    //                break;
    //            case "Comercial":
    //            case "Vaga":
    //            case "Vaga de Garagem":
    //            case "Lajes Corporativas":
    //            case "Mall":
    //            case "Salas":
    //                tipo = "Comercial/Industrial";
    //                subtipo = DeterminaSubTipoDoImovelParaTipoComercial(imovel.Subtipo);
    //                categoria = DeterminaCategoriaParaTipoComercial(imovel.Subtipo);
    //                break;
    //            case "Propriedade Rural":
    //                tipo = "Rural";
    //                subtipo = DeterminaSubTipoDoImovelParaTipoPropriedadeRural(imovel.Subtipo);
    //                categoria = "Padrão";
    //                break;
    //            case "Loteamento":
    //            case "Terreno":
    //                tipo = "Terreno";
    //                subtipo = DeterminaSubTipoDoImovelParaTipoTerreno(imovel.Subtipo);
    //                categoria = "Padrão";
    //                break;
    //            case "Flat":
    //                tipo = "Flat/Aparthotel";
    //                subtipo = "Flat";
    //                categoria = "Padrão";
    //                break;
    //            case "Apartamento":
    //            default:
    //                tipo = "Apartamento";
    //                subtipo = DeterminaSubTipoDoImovelParaTipoApartamento(imovel.Subtipo);
    //                categoria = DeterminaCategoriaParaTipoApartamento(imovel.Subtipo);
    //                break;
    //        }
    //    }

    //    private string DeterminaSubTipoDoImovelParaTipoApartamento(string subtipo)
    //    {
    //        switch (subtipo.Trim())
    //        {
    //            case "Flat":
    //            case "Loft":
    //                return "Loft";
    //            case "Kitnet":
    //                return "Kitchenette/Conjugados";
    //            case "Diferenciada":
    //            case "Padrão":
    //            case "Cobertura":
    //            case "Duplex":
    //            case "Piso único":
    //            case "Triplex":
    //            case "Cobertura Duplex":
    //            case "Cobertura Triplex":
    //            default:
    //                return "Apartamento Padrão";
    //        }
    //    }
    //    private string DeterminaCategoriaParaTipoApartamento(string subtipo)
    //    {
    //        switch (subtipo)
    //        {
    //            case "Cobertura":
    //                return "Cobertura";
    //            case "Cobertura Duplex":
    //                return "Cobertura Duplex";
    //            case "Cobertura Triplex":
    //                return "Cobertura Triplex";
    //            case "Duplex":
    //                return "Duplex";
    //            case "Triplex":
    //                return "Triplex";
    //            case "Diferenciada":
    //            case "Padrão":
    //            case "Piso único":
    //            case "Kitnet":
    //            case "Flat":
    //            case "Loft":
    //            default:
    //                return "Padrão";
    //        }
    //    }

    //    private string DeterminaSubTipoDoImovelParaTipoCasa(string subtipo)
    //    {
    //        switch (subtipo.Trim())
    //        {
    //            case "Condomínio fechado":
    //            case "Condomínio":
    //                return "Casa de Condomínio";
    //            case "Casa de vila":
    //                return "Casa de Vila";
    //            case "Assobradada":
    //            case "Cobertura":
    //            case "Diferenciada":
    //            case "Térrea":
    //            case "Sobreposta":
    //            case "Rua fechada":
    //            case "Geminada":
    //            case "Isolada":
    //            case "Frente":
    //            case "Fundos":
    //            case "Sobrado":
    //            case "Villagio":
    //            case "Padrão":
    //            default:
    //                return "Casa Padrão";
    //        }
    //    }
    //    private string DeterminaCategoriaParaTipoCasa(string subtipo)
    //    {
    //        switch (subtipo.Trim())
    //        {
    //            case "Condomínio fechado":
    //            case "Condomínio":
    //                return "Térrea";
    //            case "Casa de vila":
    //                return "Térrea";
    //            case "Assobradada":
    //            case "Cobertura":
    //            case "Sobrado":
    //                return "Sobrado/Duplex";
    //            case "Diferenciada":
    //            case "Térrea":
    //            case "Sobreposta":
    //            case "Rua fechada":
    //            case "Geminada":
    //            case "Isolada":
    //            case "Frente":
    //            case "Fundos":
    //            case "Villagio":
    //            case "Padrão":
    //            default:
    //                return "Térrea";
    //        }
    //    }

    //    private string DeterminaSubTipoDoImovelParaTipoComercial(string subtipo)
    //    {
    //        switch (subtipo.Trim())
    //        {
    //            case "Estúdio":
    //            case "Studio":
    //                return "Studio";
    //            case "Galpão":
    //                return "Galpão/Depósito/Armazém";
    //            case "Loja":
    //                return "Loja/Salão";
    //            case "Prédio inteiro":
    //                return "Prédio Inteiro";
    //            case "Sala":
    //                return "Conjunto Comercial/Sala";
    //            case "Vaga":
    //            case "Vagas cobertas":
    //                return "Box/Garagem";
    //            case "Padrão":
    //            case "Sobreposta":
    //            case "Casa":
    //            case "Cobertura":
    //            case "Diferenciada":
    //            case "Duplex":
    //            case "Triplex":
    //            case "Andar inteiro":
    //            case "Piso único":
    //            default:
    //                return "Casa Comercial";
    //        }
    //    }
    //    private string DeterminaCategoriaParaTipoComercial(string subtipo)
    //    {
    //        switch (subtipo.Trim())
    //        {
    //            case "Sobreposta":
    //            case "Duplex":
    //            case "Triplex":
    //                return "Sobrado/Duplex";
    //            case "Casa":
    //                return "Térrea";
    //            case "Galpão":
    //            case "Loja":
    //            case "Prédio inteiro":
    //            case "Sala":
    //            case "Vaga":
    //            case "Vagas cobertas":
    //            case "Padrão":
    //            case "Cobertura":
    //            case "Diferenciada":
    //            case "Andar inteiro":
    //            case "Piso único":
    //            case "Estúdio":
    //            case "Studio":
    //            default:
    //                return "Padrão";
    //        }
    //    }

    //    private string DeterminaSubTipoDoImovelParaTipoPropriedadeRural(string subtipo)
    //    {
    //        switch (subtipo)
    //        {
    //            case "Sítio":
    //                return "Sítio";
    //            case "Chácara":
    //                return "Chácara";
    //            case "Fazenda":
    //            default:
    //                return "Fazenda";
    //        }

    //    }
    //    private string DeterminaSubTipoDoImovelParaTipoTerreno(string subtipo)
    //    {
    //        switch (subtipo)
    //        {
    //            case "Loteamento":
    //            case "Condomínio fechado":
    //                return "Loteamento/Condomínio";
    //            case "Padrão":
    //            default:
    //                return "Terreno Padrão";
    //        }
    //    }
    //    private string DeterminaSubTipoDoImovelParaTipoHotel(string subtipo)
    //    {
    //        switch (subtipo.Trim())
    //        {
    //            case "Pousada":
    //                return "Pousada/Chalé";
    //            case "Padrão":
    //            case "Cobertura":
    //            case "Diferenciada":
    //            case "Duplex":
    //            case "Triplex":
    //            case "Piso único":
    //            default:
    //                return "Hotel";
    //        }
    //    }


    //    private static XmlElement AdicionarElemento(XmlDocument doc, XmlElement eImovel, string tag, string valor)
    //    {
    //        XmlElement elemento = doc.CreateElement(tag);
    //        elemento.InnerText = valor;
    //        eImovel.AppendChild(elemento);
    //        return elemento;
    //    }

    //    private static string QueryIdImovel(int idImovel) => QueryIdImovel("REO"+idImovel);
    //    private static string QueryIdImovel(string idImovelPortal) => $"Carga/Imoveis/Imovel/CodigoImovel[text()='{idImovelPortal}']";
    //    private void CriarXml()
    //    {
    //        XmlDocument doc = new XmlDocument();
    //        XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
    //        doc.AppendChild(docNode);

    //        XmlElement eCarga = doc.CreateElement("Carga");
    //        (eCarga).SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
    //        (eCarga).SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
    //        doc.AppendChild(eCarga);

    //        XmlElement eImoveis = doc.CreateElement("Imoveis");
    //        eCarga.AppendChild(eImoveis);

    //        doc.Save(CaminhoArquivo);
    //    }
    //}
}
