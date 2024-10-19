using Voteio.Entities;

namespace Voteio.Interfaces.Repository
{
    public interface IVotesRepository
    {
        void InserirAvaliacao(Votes vote);
    }
}
