using Lopes.SC.Domain.Commons;
using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.Infra.Commons;
using System.Text.RegularExpressions;

namespace Lopes.SC.ExportacaoAnuncio.Application.Services.XML
{
    public class ImovelXMLAppService : IImovelXMLAppService
    {
        private readonly IEmpresaApelidoPortalRepository _empresaApelidoPortalRepository;
        private readonly ILogger _logger;

        public ImovelXMLAppService(string caminhoPastaXmls,
                                   IEmpresaApelidoPortalRepository empresaApelidoPortalRepository,
                                   ILogger logger)
        {
            CaminhoPastaArquivos = caminhoPastaXmls;
            _empresaApelidoPortalRepository = empresaApelidoPortalRepository;
            _logger = logger;
        }


        private EmpresaApelidoPortal[] _empresaApelidoPortais;
        //TODO: acessar cache
        public EmpresaApelidoPortal[] EmpresaApelidoPortais => _empresaApelidoPortais ??= _empresaApelidoPortalRepository.Obter().ToArray();

        public string CaminhoPastaArquivos { get; }


        //public IEnumerable<PortalImoveisXML> ObterImoveisXMLs()
        //{
        //    if (!Directory.Exists(CaminhoPastaArquivos))
        //        throw new Exception($"Diretório não encontrado: {CaminhoPastaArquivos}");

        //    var portalImoveis = new List<PortalImoveisXML>();

        //    IEnumerable<string> caminhoArquivos = Directory.GetFiles(CaminhoPastaArquivos, "*.xml");
        //    foreach (string caminhoArquivo in caminhoArquivos)
        //    {
        //        int? idEmpresa = ObterIdEmpresa(caminhoArquivo);
        //        if (idEmpresa == null)
        //            _logger.Error($"Empresa não encontrado para o arquivo: {caminhoArquivo}.");

        //        Portal? portal = ObterVeiculo(caminhoArquivo);
        //        if (portal == null)
        //            _logger.Error($"Portal não encontrado para o arquivo: {caminhoArquivo}.");

        //        IPortalXMLBuilder estrategia = PortalXMLBuilder.ObterXmlBuilder(portal.Value, caminhoArquivo);

        //        IEnumerable<int> ids = estrategia.ObterIdImoveisNoXml();

        //        if (ids.Count() == 0)
        //            _logger.Error($"Nenhum imóvel encontrado para o arquivo {caminhoArquivo}.");

        //        _logger.Info($"Portal '{portal}'".PadRight(22) +
        //                     $"Empresa {idEmpresa}".PadRight(13) +
        //                     $"Quantidade imóveis encontrados: {ids.Count().ToString().PadLeft(4)}".PadRight(38) +
        //                     $"Arquivo: {caminhoArquivo}");

        //        portalImoveis.Add(new PortalImoveisXML(portal.Value, idEmpresa ?? 0, ids, caminhoArquivo));
        //    }
        //    return portalImoveis;
        //}

        //public IEnumerable<Portal> ObterPortaisImovel(int idImovel)
        //{
        //    if (!Directory.Exists(CaminhoPastaArquivos))
        //        throw new Exception($"Diretório não encontrado: {CaminhoPastaArquivos}");
        //    IEnumerable<string> caminhoArquivos = ObterArquivos();
        //    foreach (string caminhoArquivo in caminhoArquivos)
        //    {
        //        _logger.Debug($"Verificando arquivo: {caminhoArquivo.Replace(CaminhoPastaArquivos, "")}.");

        //        int? idEmpresa = ObterIdEmpresa(caminhoArquivo);
        //        if (idEmpresa == null)
        //            _logger.Error($"Empresa não encontrado para o arquivo: {caminhoArquivo}.");

        //        Portal? portal = ObterVeiculo(caminhoArquivo);
        //        if (portal == null)
        //        {
        //            _logger.Error($"Portal não encontrado para o arquivo: {caminhoArquivo}.");
        //            continue;
        //        }

        //        IPortalXMLBuilder estrategia = PortalXMLBuilder.ObterXmlBuilder(portal.Value, caminhoArquivo);

        //        if (estrategia.ImovelNoXml(idImovel))
        //        {
        //            _logger.Info($"Imóvel encontrado para o arquivo {caminhoArquivo}.");
        //            yield return portal.Value;
        //        }
        //    }
        //}


        public int? ObterIdEmpresa(string caminhoArquivo)
        {
            Match match = Regex.Match(caminhoArquivo, @"\\([^\\-]+)-([^.]+)");
            if (match.Success)
            {
                string apelidoEmpresa = match.Groups[2].Value;
                int? idEmpresa = EmpresaApelidoPortais.Where(_ => _.Apelido.ToLower() == apelidoEmpresa.ToLower())
                                                      .Select(_ => _.IdEmpresa)
                                                      .FirstOrDefault(_ => _ > 0);
                return idEmpresa == 0 ? null : idEmpresa;
            }

            return null;
        }

        public Portal? ObterVeiculo(string caminhoArquivo)
        {
            Match match = Regex.Match(caminhoArquivo, @"\\([^\\-]+)-");
            if (match.Success)
            {
                string portalExtenso = match.Groups[1].Value;
                Portal portal = (Portal)Enum.Parse(typeof(Portal), portalExtenso, true);
                return portal;
            }

            return null;
        }

        public IRetorno<string> CaminhoArquivoXml(Portal portal, int idEmpresa)
        {
            IRetorno<string> retorno = new Retorno<string>();

            string? apelidoEmpresa = EmpresaApelidoPortais.Where(_ => _.IdEmpresa == idEmpresa)
                                                          .Select(_ => _.Apelido)
                                                          .FirstOrDefault();
            if (string.IsNullOrEmpty(apelidoEmpresa))
                retorno.AdicionarErro($"Apelido empresa não encontrado. Id Empresa: {idEmpresa}");

            string nomePortal = Enum.GetName(portal);
            if (string.IsNullOrEmpty(nomePortal))
                retorno.AdicionarErro($"Portal não encontrado. Id portal: {portal}");

            if (!retorno.Sucesso)
                return retorno;

            string caminhoArquivo = CaminhoPastaArquivos + "/" + nomePortal.ToLower() + "-" + apelidoEmpresa + ".xml";

            return retorno.AdicionarDado(caminhoArquivo);
        }


        private IEnumerable<string> ObterArquivos() => Directory.GetFiles(CaminhoPastaArquivos, "*.xml");

    }
}