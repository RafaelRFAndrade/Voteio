using Voteio.Messaging.Enums;

namespace Voteio.Messaging.Requests
{
    public class AvaliarIdeiaRequest
    {
        public Guid CodigoIdeia {  get; set; }
        public TipoVote TipoVote { get; set; }
    }
}
