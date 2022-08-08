using Lopes.SC.Anuncio.Domain.Enums;
using Lopes.SC.Anuncio.Domain.Reposities;

namespace Lopes.SC.Anuncio.Domain.Services
{
    public class StatusAnuncioService : IStatusAnuncioService
    {
        private readonly IImovelRepository _imovelRepository;
        private readonly IDictionary<int, int[]> _imovelEmpresasCache;

        public StatusAnuncioService(IImovelRepository imovelRepository)
        {
            _imovelRepository = imovelRepository;
            _imovelEmpresasCache = new Dictionary<int, int[]>();
        }


        public StatusAnuncioPortal VerificarStatusImovelPortal(Models.Anuncio anuncio, bool imovelNoXml)
        {
            if (!anuncio.Ativo ||
                !anuncio.CotaAtiva ||
                !anuncio.ImovelAtivo ||
                !PodeAnunciarOutraEmpresa(anuncio))
            {
                return imovelNoXml
                    ? StatusAnuncioPortal.ARemover
                    : StatusAnuncioPortal.Removido;
            }

            if (anuncio.AnuncioDesatualizado || !imovelNoXml)
                return StatusAnuncioPortal.Desatualizado;

            return StatusAnuncioPortal.Atualizado;
        }

        private bool PodeAnunciarOutraEmpresa(Models.Anuncio anuncio)
        {
            if (anuncio.PodeAnunciarOutraEmpresa)
                return true;

            int[] idEmpresas = ObterEmpresasImovel(anuncio.IdImovel);
            return idEmpresas.Contains(anuncio.IdEmpresa);
        }

        public int[] ObterEmpresasImovel(int idImovel)
        {
            if (_imovelEmpresasCache.TryGetValue(idImovel, out int[] idEmpresas))
                return idEmpresas;

            idEmpresas = _imovelRepository.ObterEmpresasImovel(idImovel).ToArray();
            _imovelEmpresasCache.Add(idImovel, idEmpresas);

            return idEmpresas;
        }
    }
}
