namespace Julio.Domain.Commons
{
    public interface IResultadoItens
    {
        int ItensSucesso { get; }
        int ItensErro { get; }
        int TotalItens { get; }
        string[] Erros { get; }

        IResultadoItens Add(string mensagemErro);
        IResultadoItens Add(IResultado resultado);
        IResultadoItens Add(bool sucesso);
        IResultadoItens Add(IResultadoItens resultadoItens);
    }

    public class ResultadoItens : IResultadoItens
    {
        private List<string> erros = new();


        public int ItensSucesso { get; set; }
        public int ItensErro { get; set; }
        public int TotalItens => ItensSucesso + ItensErro;
        public string[] Erros => erros.ToArray();


        public IResultadoItens Add(IResultado resultado)
        {
            if (resultado.Sucesso)
                ItensSucesso++;
            else
                Add(resultado.Mensagens.Where(_ => _.Tipo == TipoMensagem.Erro)
                                       .Select(_ => _.Descricao)
                                       .ToArray());
            return this;
        }
        public IResultadoItens Add(bool sucesso)
        {
            if (sucesso)
                ItensSucesso++;
            else
                ItensErro++;

            return this;
        }

        public IResultadoItens Add(string[] mensagemErro)
        {
            erros.AddRange(mensagemErro);
            ItensErro += mensagemErro.Length;

            return this;
        }
        public IResultadoItens Add(string mensagemErro)
        {
            erros.Add(mensagemErro);
            ItensErro++;

            return this;
        }

        public IResultadoItens Add(IResultadoItens resultadoItens)
        {
            ItensSucesso = resultadoItens.ItensSucesso;
            ItensErro = resultadoItens.ItensErro;
            erros = resultadoItens.Erros.ToList();

            return this;
        }
    }
}