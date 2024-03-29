﻿using Julio.Acesso.Commons.Extensions;
using Julio.Anuncio.Application.DadosService;
using Julio.Anuncio.Application.Models;
using Julio.Anuncio.Domain.Commands.Requests;
using Julio.Anuncio.Domain.Enums;
using Julio.Anuncio.Domain.ObjetosValor;
using Julio.Anuncio.Domain.Services;
using static Julio.Anuncio.Application.Models.AnunciosDesatualizadosViewModel;

namespace Julio.Anuncio.Application.Services
{
    public class CotaAppService : ICotaAppService
    {
        private readonly ICotaDadosService _dadosService;
        private readonly IStatusAnuncioService _statusAnuncioService;
        private readonly IAnuncioDadosService _anuncioDadosService;
        private readonly IPortalAtualizadorFactory _portalAtualizadorFactory;

        public CotaAppService(ICotaDadosService dadosService,
                              IStatusAnuncioService statusAnuncioService,
                              IAnuncioDadosService anuncioDadosService,
                              IPortalAtualizadorFactory portalAtualizadorFactory)
        {
            _dadosService = dadosService;
            _statusAnuncioService = statusAnuncioService;
            _anuncioDadosService = anuncioDadosService;
            _portalAtualizadorFactory = portalAtualizadorFactory;
        }

        public AnunciosDesatualizadosViewModel ObterAnunciosDesatualizadosPorCota(int idCota)
        {
            IEnumerable<AnuncioCota> anuncios = _anuncioDadosService.Obter(new AnuncioCotaRequest(idCotas: new[] { idCota })).ToList();

            if (anuncios.Nenhum())
                throw new Exception($"Nenhum anúncio encontrado para a cota {idCota}.");

            Portal portal = anuncios.First().Portal;
            int idFranquia = anuncios.First().IdFranquia;
            IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador(portal, idFranquia);
            List<int> idProdutosNoPortal = atualizador.ObterIdProdutosNoPortal().ToList();

            var franquiaAnunciosDesatualizados = new FranquiaAnunciosDesatualizados(idFranquia, idCota);

            foreach (AnuncioCota anuncio in anuncios)
            {
                bool produtoNoPortal = idProdutosNoPortal.Contains(anuncio.IdProduto);
                StatusAnuncioPortal statusAnuncioPortal = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, produtoNoPortal);
                if (statusAnuncioPortal == StatusAnuncioPortal.Desatualizado)
                    franquiaAnunciosDesatualizados.QtdeAnunciosParaIncluirAtualizar++;
                else if (statusAnuncioPortal == StatusAnuncioPortal.ARemover)
                    franquiaAnunciosDesatualizados.QtdeAnunciosParaExcluir++;
            }

            return new AnunciosDesatualizadosViewModel((int)portal, franquiaAnunciosDesatualizados);
        }

        public AnunciosDesatualizadosViewModel ObterAnunciosDesatualizadosPorPortal(int portal)
        {
            var franquiasAnuncios = _anuncioDadosService.Obter(new AnuncioCotaRequest((Portal)portal))
                .GroupBy(_ => new { _.IdFranquia, _.IdCota})
                .Select(_ => new { IdFranquia = _.Key.IdFranquia, IdCota = _.Key.IdCota, Anuncios = _.ToList() })
                .ToList();

            var anunciosDesatualizados = new AnunciosDesatualizadosViewModel(portal);

            foreach (var franquiaAnuncios in franquiasAnuncios)
            {
                IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador((Portal)portal, franquiaAnuncios.IdFranquia);
                var idProdutosNoPortal = atualizador.ObterIdProdutosNoPortal().ToList();

                var franquiaAnunciosDesatualizados = new FranquiaAnunciosDesatualizados(franquiaAnuncios.IdFranquia, franquiaAnuncios.IdCota);

                foreach (AnuncioCota? anuncio in franquiaAnuncios.Anuncios)
                {
                    bool produtoNoPortal = idProdutosNoPortal.Contains(anuncio.IdProduto);
                    StatusAnuncioPortal statusAnuncioPortal = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, produtoNoPortal);
                    if (statusAnuncioPortal == StatusAnuncioPortal.Desatualizado)
                        franquiaAnunciosDesatualizados.QtdeAnunciosParaIncluirAtualizar++;
                    else if (statusAnuncioPortal == StatusAnuncioPortal.ARemover)
                        franquiaAnunciosDesatualizados.QtdeAnunciosParaExcluir++;
                }

                anunciosDesatualizados.AnunciosDesatualizados.Add(franquiaAnunciosDesatualizados);
            }

            return anunciosDesatualizados;
        }

        public IEnumerable<CotaResumoViewModel> ObterCotas()
        {
            List<CotaResumo> todasCotas = _dadosService.Obter(new CotaResumoRequest()).ToList();

            List<CotaResumo> cotasFranquiaPortal = SelecionarCotaFranquiaPortal(todasCotas);

            return cotasFranquiaPortal.Select(_ => new CotaResumoViewModel(_));
        }

        /// <summary>
        /// Retorna somente uma cota de uma mesma franquia e portal (a mais ativa)
        /// </summary>
        /// <param name="todasCotas">Cotas</param>
        /// <returns></returns>
        private static List<CotaResumo> SelecionarCotaFranquiaPortal(List<CotaResumo> todasCotas)
        {
            List<CotaResumo> cotasFranquiaPortal = new List<CotaResumo>();

            foreach (CotaResumo cota in todasCotas.OrderBy(_ => _.IdStatusCota))
            {
                //Cada Franquia só pode ter uma cota de cada portal
                if (!cotasFranquiaPortal.Any(_ => _.IdFranquia == cota.IdFranquia && _.Portal == cota.Portal))
                    cotasFranquiaPortal.Add(cota);
            }
            return cotasFranquiaPortal;
        }
    }
}
