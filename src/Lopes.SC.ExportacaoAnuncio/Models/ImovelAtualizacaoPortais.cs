using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Models
{
    public class ImovelAtualizadoPortais : ImovelAtualizacaoPortais
    {
        public ImovelAtualizadoPortais(int idImovel, Portal veiculo, DateTime dataAtualizacao) 
            : base(idImovel, veiculo, dataAtualizacao, null)
        {
        }
    }
    public class ImovelRemovidoPortais : ImovelAtualizacaoPortais
    {
        public ImovelRemovidoPortais(int idImovel, Portal veiculo, DateTime dataExclusao) 
            : base(idImovel, veiculo, null, dataExclusao)
        {
        }
    }

    public class ImovelAtualizacaoPortais
    {
        public ImovelAtualizacaoPortais(int idImovel, Portal veiculo, DateTime? dataAtualizacao, DateTime? dataExclusao)
        {
            IdImovel = idImovel;
            IdPortal = veiculo;
            DataAtualizacao = dataAtualizacao;
            DataRemocao = dataExclusao;
        }

        protected ImovelAtualizacaoPortais()
        {
            //ctor para o EF
        }

        public int Id { get; set; }

        public int IdImovel { get; set; }
        public int IdEmpresa { get; set; }
        public Portal IdPortal { get; set; }

        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataRemocao { get; set; }
    }
}
