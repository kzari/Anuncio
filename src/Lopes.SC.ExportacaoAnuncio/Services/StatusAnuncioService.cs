using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Services
{
    public class StatusAnuncioService : IStatusAnuncioService
    {
        private readonly IImovelRepository _imovelRepository;
        private readonly IDictionary<int, int[]> _empresaImoveisCache;

        public StatusAnuncioService(IImovelRepository imovelRepository)
        {
            _imovelRepository = imovelRepository;
            _empresaImoveisCache = new Dictionary<int, int[]>();
        }


        public StatusImovelPortal VerificarStatusImovelPortal(Anuncio anuncio, params Portal[] portaisInseridos)
        {
            bool imovelNoXml = portaisInseridos != null && portaisInseridos.Contains(anuncio.Portal);
            return VerificarStatusImovelPortal(anuncio, imovelNoXml);
        }

        public StatusImovelPortal VerificarStatusImovelPortal(Anuncio anuncio, bool imovelNoXml)
        {
            if (!ImovelAtivo(anuncio) ||
                !CotaAtiva(anuncio) ||
                !AnuncioAtivo(anuncio))
            {
                return imovelNoXml ? StatusImovelPortal.ARemover : StatusImovelPortal.Removido;
            }

            if (!anuncio.PodeAnunciarOutraLoja)
            {
                int[] empresasImovel = ObterEmpresasImovel(anuncio);
                if (!empresasImovel.Contains(anuncio.IdEmpresa))
                    return imovelNoXml ? StatusImovelPortal.ARemover : StatusImovelPortal.Atualizado;
            }

            if (anuncio.ImovelAtualizado || !imovelNoXml)
                return StatusImovelPortal.Desatualizado;

            return StatusImovelPortal.Atualizado;
        }

        private static bool AnuncioAtivo(Anuncio anuncio) => anuncio.IdAnuncioStatus == 1 && anuncio.AnuncioLiberado;
        private static bool ImovelAtivo(Anuncio anuncio)  => anuncio.StatusImovel == StatusImovel.Ativo;
        private static bool CotaAtiva(Anuncio anuncio)    => anuncio.IdCotaStatus == 1;


        private int[] ObterEmpresasImovel(Anuncio anuncio)
        {
            //return new int[0]; //TODO: remover

            if (_empresaImoveisCache.TryGetValue(anuncio.IdImovel, out int[] idEmpresas))
                return idEmpresas;

            idEmpresas = _imovelRepository.ObterEmpresasImovel(anuncio.IdImovel);
            _empresaImoveisCache.Add(anuncio.IdImovel, idEmpresas);

            return idEmpresas;
        }
    }
}
