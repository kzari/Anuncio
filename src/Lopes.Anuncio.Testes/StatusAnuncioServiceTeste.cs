using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Domain.Services;
using Lopes.Anuncio.Domain.Testes.Mocks;

namespace Lopes.SC.Anuncio.Domain.Testes
{
    public class StatusAnuncioServiceTeste
    {
        private readonly StatusAnuncioService _statusAnuncioService;

        public StatusAnuncioServiceTeste()
        {
            //_statusAnuncioService = new StatusAnuncioService(new ProdutoRepositoryMock());
        }


        [Fact]
        public void VerificarAnuncioDesatualizado()
        {
            var anuncioDesatualizado = new AnuncioCota()
            {
                IdFranquia = 1,
                DataAtualizacaoAnuncioPortal = DateTime.Now.AddDays(-1),
                ProdutoUltimaAlteracao = DateTime.Now,
                IdStatusAnuncio = 1,
                IdStatusCota = 1,
                IdStatusProduto = ProdutoStatus.Ativo
            };
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusProdutoPortal(anuncioDesatualizado, true);
            Assert.True(status == StatusAnuncioPortal.Desatualizado);
        }

        [Fact]
        public void VerificarAnuncioAtualizado()
        {
            AnuncioCota anuncioAtualizado = AnuncioAtualizado();
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusProdutoPortal(anuncioAtualizado, true);
            Assert.True(status == StatusAnuncioPortal.Atualizado);
        }

        [Fact]
        public void VerificarAnuncioParaRemover_CotaInativa()
        {
            var anuncio = AnuncioAtualizado();
            anuncio.IdStatusCota = 2;
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, true);
            Assert.True(status == StatusAnuncioPortal.ARemover);

            StatusAnuncioPortal status2 = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, false);
            Assert.True(status2 == StatusAnuncioPortal.Removido);
        }

        [Fact]
        public void VerificarAnuncioParaRemover_AnuncioInativo()
        {
            var anuncio = AnuncioAtualizado();
            anuncio.IdStatusAnuncio = 2;
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, true);
            Assert.True(status == StatusAnuncioPortal.ARemover);

            StatusAnuncioPortal status2 = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, false);
            Assert.True(status2 == StatusAnuncioPortal.Removido);
        }

        [Fact]
        public void VerificarAnuncioParaRemover_ProdutoInativo()
        {
            var anuncio = AnuncioAtualizado();
            anuncio.IdStatusProduto = ProdutoStatus.Baixado;
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, true);
            Assert.True(status == StatusAnuncioPortal.ARemover);

            StatusAnuncioPortal status2 = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, false);
            Assert.True(status2 == StatusAnuncioPortal.Removido);
        }

        [Fact]
        public void VerificarAnuncioParaAtualizar_PodeAnunciarOutraEmpresa()
        {
            var anuncio = AnuncioAtualizado();
            anuncio.DataAtualizacaoAnuncioPortal = null;
            anuncio.PodeAnunciarOutraFranquia = true;
            StatusAnuncioPortal status = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, true);
            Assert.True(status == StatusAnuncioPortal.Desatualizado);

            anuncio.PodeAnunciarOutraFranquia = false;
            anuncio.IdFranquia = 1;
            StatusAnuncioPortal status2 = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, true);
            Assert.True(status2 == StatusAnuncioPortal.Desatualizado);

            anuncio.IdFranquia = 2;
            StatusAnuncioPortal status3 = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, true);
            Assert.True(status3 == StatusAnuncioPortal.ARemover);

            StatusAnuncioPortal status4 = _statusAnuncioService.VerificarStatusProdutoPortal(anuncio, false);
            Assert.True(status4 == StatusAnuncioPortal.Removido);
        }




        private static AnuncioCota AnuncioAtualizado()
        {
            var now = DateTime.Now;
            var anuncioAtualizado = new AnuncioCota()
            {
                IdFranquia = 1,
                DataAtualizacaoAnuncioPortal = now,
                ProdutoUltimaAlteracao = now,
                IdStatusAnuncio = 1,
                IdStatusCota = 1,
                IdStatusProduto = ProdutoStatus.Ativo
            };
            return anuncioAtualizado;
        }
    }
}