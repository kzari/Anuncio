using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Domain.Commons.Cache;

namespace Lopes.Anuncio.Domain.Services
{
    public class StatusAnuncioService : IStatusAnuncioService
    {
        private const string CHAVE_CACHE_FILIAIS_PRODUTO = "FranquiasProduto_[idProduto]";
        private readonly IProdutoDadosService _produtoDadosService;
        private readonly ICacheService _cacheService;

        public StatusAnuncioService(IProdutoDadosService produtoDadosService, ICacheService cacheService)
        {
            _produtoDadosService = produtoDadosService;
            _cacheService = cacheService;
        }


        public StatusAnuncioPortal VerificarStatusImovelPortal(AnuncioCota anuncio, bool imovelNoXml)
        {
            if (!anuncio.Ativo ||
                !anuncio.CotaAtiva ||
                !anuncio.ProdutoAtivo ||
                !PodeAnunciarOutraFranquia(anuncio))
            {
                return imovelNoXml
                    ? StatusAnuncioPortal.ARemover
                    : StatusAnuncioPortal.Removido;
            }

            if (anuncio.AnuncioDesatualizado || !imovelNoXml)
                return StatusAnuncioPortal.Desatualizado;

            return StatusAnuncioPortal.Atualizado;
        }

        private bool PodeAnunciarOutraFranquia(AnuncioCota anuncio)
        {
            if (anuncio.PodeAnunciarOutraFranquia)
                return true;

            string chave = CHAVE_CACHE_FILIAIS_PRODUTO.Replace("[idProduto]", anuncio.IdProduto.ToString());

            int[] idFranquias = _cacheService.ObterOuGravar(CHAVE_CACHE_FILIAIS_PRODUTO, TimeSpan.FromDays(1), () =>
            {
                return _produtoDadosService.ObterFranquias(anuncio.IdProduto);
            });

            return idFranquias?.Contains(anuncio.IdFranquia) ?? true;
        }
    }
}
