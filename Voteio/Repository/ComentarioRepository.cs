using Microsoft.EntityFrameworkCore;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
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
    }
}
