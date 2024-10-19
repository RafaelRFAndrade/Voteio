using Voteio.Entities;
using Voteio.Messaging.RawQuery;

namespace Voteio.Interfaces.Repository
{
    public interface IVotesRepository
    {
        void InserirAvaliacao(Votes vote);
        CountRawQuery ValidarSeJaFoiVotadoPorUsuario(Guid codigoUsuario);
    }
}
