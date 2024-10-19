using Microsoft.EntityFrameworkCore;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Repository.Base;

namespace Voteio.Repository
{
    public class VotesRepository : RepositoryBase, IVotesRepository
    {
        public VotesRepository(DbContextOptions<RepositoryBase> options) : base(options)
        {
        }

        public void InserirAvaliacao(Votes vote)
        {
            Add(vote);
            SaveChanges();
        }
    }
}
