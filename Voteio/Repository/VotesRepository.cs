using Microsoft.EntityFrameworkCore;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.RawQuery;
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

        public CountRawQuery ValidarSeJaFoiVotadoPorUsuario(Guid codigoUsuario)
        {
            const string sql = @"
                SELECT 
                    COUNT(1) as 'Count'
                FROM    
                    Voteio.Votes
                WHERE 
                    CodigoUsuario = @p0 ";

            return Database.SqlQueryRaw<CountRawQuery>(sql, codigoUsuario).FirstOrDefault();
        }
    }
}
