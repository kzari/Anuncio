﻿using Lopes.Anuncio.Application.DadosService;
using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Domain.Services;
using Lopes.Domain.Commons;
using Lopes.Domain.Commons.Cache;
using Lopes.Infra.XML;
using Microsoft.Extensions.Configuration;

namespace Lopes.Anuncio.IoC
{
    public class PortalAtualizadorFactory : IPortalAtualizadorFactory
    {
        private const string CHAVE_CACHE_CARACTERISTICAS_PORTAL = "CaracteristicasPortal_[portal]";
        private const string CHAVE_CACHE_APELIDO_EMPRESAS = "ApelidoEmpresas";
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;


        public PortalAtualizadorFactory(IServiceProvider serviceProvider, 
                                        IConfiguration configuration, 
                                        ICacheService cacheService)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _cacheService = cacheService;
        }


        public IPortalAtualizador ObterAtualizador(Portal portal, int idEmpresa)
        {
            switch (portal)
            {
                case Portal.ProdutoWeb:
                    return new ProdutoWebApi();

                default:
                    {
                        string caminhoPastaXmls = _configuration["CaminhoPastaXmls"];
                        string urlImagens = _configuration["UrlFotosProdutos"];

                        IEnumerable<PortalCaracteristica> portalCaracteristicas = ObterCaracteristicasPortal(portal) ?? Enumerable.Empty<PortalCaracteristica>();
                        string apelidoEmpresa = ObterApelidoEmpresa(idEmpresa);

                        return new PortalXMLBuilder(caminhoPastaXmls, portalCaracteristicas, apelidoEmpresa, portal, idEmpresa, urlImagens);
                    }
            }
        }


        private string ObterApelidoEmpresa(int idEmpresa)
        {
            IEnumerable<FranquiaApelido>? apelidos = _cacheService.ObterOuGravar(CHAVE_CACHE_APELIDO_EMPRESAS,
                                                                                 TimeSpan.FromDays(1),
                                                                                 () => _serviceProvider.ObterServico<IFranquiaApelidoPortalDadosAppService>().Obter())?.ToList();
            return apelidos?.FirstOrDefault(_ => _.IdEmpresa == idEmpresa)?.Apelido ?? string.Empty;
        }

        private IEnumerable<PortalCaracteristica>? ObterCaracteristicasPortal(Portal portal)
        {
            string chave = CHAVE_CACHE_CARACTERISTICAS_PORTAL.Replace("[portal]", portal.ToString());

            return _cacheService.ObterOuGravar(chave,
                                               TimeSpan.FromDays(1),
                                               () => _serviceProvider.ObterServico<IPortalCaracteristicaDadosAppService>().Obter(portal))?.ToList();
        }
    }
}
