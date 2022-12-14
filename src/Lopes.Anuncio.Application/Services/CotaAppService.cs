using Lopes.Acesso.Commons.Extensions;
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
            return _dadosService.Obter(new CotaResumoRequest(68)).ToList().Select(_ => new CotaResumoViewModel(_));
        }
    }
}
