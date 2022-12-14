using Lopes.Anuncio.Domain.Commands.Requests;

namespace Lopes.Anuncio.Domain.Commands.Responses
{
    public class AtualizarStatusAnuncioResponse
    {
        public AtualizarStatusAnuncioResponse(RegistroAtualizacaoCommand request)
        {
            Id = request.Id;
            DataHora = request.Data;
        }
        public Guid Id { get; set; }
        public DateTime DataHora { get; set; }
    }
}
