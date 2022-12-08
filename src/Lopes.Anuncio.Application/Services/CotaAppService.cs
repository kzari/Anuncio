using Lopes.Anuncio.Application.DadosService;
using Lopes.Anuncio.Application.Models;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Domain.Services;
using static Lopes.Anuncio.Application.Models.AnunciosDesatualizadosViewModel;

namespace Lopes.Anuncio.Application.Services
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

        public AnunciosDesatualizadosViewModel ObterAnunciosDesatualizados(int portal)
        {
            var franquiasAnuncios = _anuncioDadosService.Obter(new AnuncioCotaRequest((Portal)portal))
                .GroupBy(_ => _.IdFranquia)
                .Select(_ => new { IdFranquia = _.Key, Anuncios = _.ToList() })
                .ToList();

            var anunciosDesatualizados = new AnunciosDesatualizadosViewModel(portal);

            foreach (var franquiaAnuncios in franquiasAnuncios)
            {
                IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador((Portal)portal, franquiaAnuncios.IdFranquia);
                var idProdutosNoPortal = atualizador.ObterIdProdutosNoPortal().ToList();

                var franquiaAnunciosDesatualizados = new FranquiaAnunciosDesatualizados(franquiaAnuncios.IdFranquia);

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
            return _dadosService.Obter(new CotaResumoRequest(68)).ToList().Select(_ => new CotaResumoViewModel(_));
        }
    }
}
