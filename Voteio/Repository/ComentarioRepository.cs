using Microsoft.EntityFrameworkCore;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.RawQuery;
using Voteio.Messaging.Responses;
using Voteio.Repository.Base;

namespace Voteio.Repository
{
    public class ComentarioRepository : RepositoryBase, IComentarioRepository
    {
        public ComentarioRepository(DbContextOptions<RepositoryBase> options) : base(options)
        {
        }

        public void InserirComentario(Comentario comentario)
        {
            Add(comentario);
            SaveChanges();
        }

        public List<ComentarioDto> ObterComentariosPorIdeia(Guid codigoIdeia)
        {
            string sql = @" SELECT 
                                usu.Codigo as 'CodigoUsuario',
                                usu.Nome as 'NomeUsuario',
                                usu.Email,
                                com.Conteudo as 'Comentario'
                            FROM
                                Voteio.Comentario as com
                            LEFT JOIN 
                                Voteio.Usuario as usu on usu.Codigo = com.CodigoUsuario
                            WHERE 
                                CodigoIdeia = @p0 ";

            return Database.SqlQueryRaw<ComentarioDto>(sql, codigoIdeia).ToList();
        }
    }
}
