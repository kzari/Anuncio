using Lopes.SC.Anuncio.Domain.Enums;
using Lopes.SC.Anuncio.Domain.Services;
using Lopes.SC.Anuncio.Domain.Testes.Mocks;

namespace Lopes.SC.Anuncio.Domain.Testes
{
    public class StatusAnuncioServiceTeste
    {
        private readonly StatusAnuncioService _statusAnuncioService;

        public StatusAnuncioServiceTeste()
        {
            _statusAnuncioService = new StatusAnuncioService(new ImovelRepositoryMock());
        }


        [Fact]
        public void VerificarAnuncioDesatualizado()
        {
            var anuncioDesatualizado = new Models.Anuncio()
            {
                IdEmpresa = 1,
                DataAtualizacaoAnuncioPortal = DateTime.Now.AddDays(-1),
                ImovelUltimaAlteracao = DateTime.Now,
                IdStatusAnuncio = 1,
                IdStatusCota = 1,
                IdStatusImovel = StatusImovel.Ativo
            };
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusImovelPortal(anuncioDesatualizado, true);
            Assert.True(status == StatusAnuncioPortal.Desatualizado);
        }

        [Fact]
        public void VerificarAnuncioAtualizado()
        {
            Models.Anuncio anuncioAtualizado = AnuncioAtualizado();
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusImovelPortal(anuncioAtualizado, true);
            Assert.True(status == StatusAnuncioPortal.Atualizado);
        }

        [Fact]
        public void VerificarAnuncioParaRemover_CotaInativa()
        {
            var anuncio = AnuncioAtualizado();
            anuncio.IdStatusCota = 2;
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, true);
            Assert.True(status == StatusAnuncioPortal.ARemover);

            StatusAnuncioPortal status2 = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, false);
            Assert.True(status2 == StatusAnuncioPortal.Removido);
        }

        [Fact]
        public void VerificarAnuncioParaRemover_AnuncioInativo()
        {
            var anuncio = AnuncioAtualizado();
            anuncio.IdStatusAnuncio = 2;
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, true);
            Assert.True(status == StatusAnuncioPortal.ARemover);

            StatusAnuncioPortal status2 = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, false);
            Assert.True(status2 == StatusAnuncioPortal.Removido);
        }

        [Fact]
        public void VerificarAnuncioParaRemover_ImovelInativo()
        {
            var anuncio = AnuncioAtualizado();
            anuncio.IdStatusImovel = StatusImovel.Baixado;
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, true);
            Assert.True(status == StatusAnuncioPortal.ARemover);

            StatusAnuncioPortal status2 = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, false);
            Assert.True(status2 == StatusAnuncioPortal.Removido);
        }

        [Fact]
        public void VerificarAnuncioParaAtualizar_PodeAnunciarOutraEmpresa()
        {
            var anuncio = AnuncioAtualizado();
            anuncio.DataAtualizacaoAnuncioPortal = null;
            anuncio.PodeAnunciarOutraEmpresa = true;
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, true);
            Assert.True(status == StatusAnuncioPortal.Desatualizado);

            anuncio.PodeAnunciarOutraEmpresa = false;
            anuncio.IdEmpresa = 1;
            StatusAnuncioPortal status2 = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, true);
            Assert.True(status2 == StatusAnuncioPortal.Desatualizado);

            anuncio.IdEmpresa = 2;
            StatusAnuncioPortal status3 = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, true);
            Assert.True(status3 == StatusAnuncioPortal.ARemover);

            StatusAnuncioPortal status4 = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, false);
            Assert.True(status4 == StatusAnuncioPortal.Removido);
        }




        private static Models.Anuncio AnuncioAtualizado()
        {
            var now = DateTime.Now;
            var anuncioAtualizado = new Models.Anuncio()
            {
                IdEmpresa = 1,
                DataAtualizacaoAnuncioPortal = now,
                ImovelUltimaAlteracao = now,
                IdStatusAnuncio = 1,
                IdStatusCota = 1,
                IdStatusImovel = StatusImovel.Ativo
            };
            return anuncioAtualizado;
        }
    }
}