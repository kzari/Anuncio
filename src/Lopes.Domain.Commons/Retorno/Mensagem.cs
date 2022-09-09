namespace Lopes.Domain.Commons
{
    public struct Mensagem
    {
        public Mensagem(TipoMensagem tipo, string descricao)
        {
            Tipo = tipo;
            Descricao = descricao;
        }

        public TipoMensagem Tipo { get; }
        public string Descricao { get; }
    }
}