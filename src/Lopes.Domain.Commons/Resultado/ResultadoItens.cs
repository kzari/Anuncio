namespace Lopes.Domain.Commons
{
    public interface IResultadoItens
    {
        int ItensSucesso { get; }
        int ItensErro { get; }
        int TotalItens { get; }
        string[] Erros { get; }

        void Add(string mensagemErro);
        void Add(IResultado resultado);
        void Add(bool sucesso);
    }

    public class ResultadoItens : IResultadoItens
    {
        private List<string> erros = new();


        public int ItensSucesso { get; set; }
        public int ItensErro { get; set; }
        public int TotalItens => ItensSucesso + ItensErro;
        public string[] Erros => erros.ToArray();


        public void Add(IResultado resultado)
        {
            if (resultado.Sucesso)
                ItensSucesso++;
            else
                Add(resultado.Mensagens.Where(_ => _.Tipo == TipoMensagem.Erro)
                                       .Select(_ => _.Descricao)
                                       .ToArray());
        }
        public void Add(bool sucesso)
        {
            if (sucesso)
                ItensSucesso++;
            else
                ItensErro++;
        }

        public void Add(string[] mensagemErro)
        {
            erros.AddRange(mensagemErro);
            ItensErro += mensagemErro.Length;
        }
        public void Add(string mensagemErro)
        {
            erros.Add(mensagemErro);
            ItensErro++;
        }
    }
}