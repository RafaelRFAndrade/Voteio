using Microsoft.EntityFrameworkCore;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Repository.Base;

namespace Voteio.Repository
{
    public class UsuarioRepository : RepositoryBase, IUsuarioRepository
    {
        public UsuarioRepository(DbContextOptions<RepositoryBase> options) : base(options)
        {
        }

        public void InserirUsuario(Usuario usuario)
        {
           Add(usuario);
           SaveChanges();
        }

        public List<Usuario> ListarUsuarios()
        {
            return Usuario.ToList();
        }

        public Usuario ObterPorEmail(string email)
        {
            const string sql = @"
                    SELECT 
                        * 
                    FROM 
                        Voteio.Usuario 
                    WHERE 
                        Email = @p0 ";

            return Usuario.FromSqlRaw(sql, email).FirstOrDefault();
        }

        public Usuario ObterPorId(string userId)
        {
            return Find<Usuario>(long.Parse(userId));
        }
    }
}
