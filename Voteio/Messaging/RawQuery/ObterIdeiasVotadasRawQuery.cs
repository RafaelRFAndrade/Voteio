using Voteio.Messaging.Enums;

namespace Voteio.Messaging.RawQuery
{
    public class ObterIdeiasVotadasRawQuery
    {
        public Guid CodigoIdeia { get; set; }
        public TipoVote TipoVote { get; set; }
    }
}
