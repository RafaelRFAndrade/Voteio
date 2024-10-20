using Microsoft.EntityFrameworkCore;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.Enums;
using Voteio.Messaging.RawQuery;
using Voteio.Repository.Base;

namespace Voteio.Repository
{
    public class IdeiasRepository : RepositoryBase, IIdeiasRepository
    {
        public IdeiasRepository(DbContextOptions<RepositoryBase> options) : base(options)
        {
        }

        public void InserirIdeia(Ideias ideia)
        {
            Add(ideia);
            SaveChanges();
        }

        public List<ObterIdeiasRawQuery> ObterIdeias()
        {
            string sql = $@"
                SELECT
                    ie.Codigo,
                    ie.Titulo,
                    ie.Descricao,
                    (
                        SELECT COUNT(1) 
                        FROM Voteio.Votes vt1 
                        WHERE vt1.CodigoIdeia = ie.Codigo AND vt1.TipoVote = {(int)TipoVote.Upvote}
                    ) AS Upvotes,
                    (
                        SELECT COUNT(1) 
                        FROM Voteio.Votes vt2 
                        WHERE vt2.CodigoIdeia = ie.Codigo AND vt2.TipoVote = {(int)TipoVote.Downvote}
                    ) AS Downvotes,
                    usu.Nome as NomeUsuario
                FROM
                    Voteio.Ideias AS ie 
                INNER JOIN 
                    Voteio.Usuario as usu on ie.CodigoUsuario = usu.Codigo ";

            return Database.SqlQueryRaw<ObterIdeiasRawQuery>(sql).ToList();
        }
    }
}
