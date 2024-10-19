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
    }
}
