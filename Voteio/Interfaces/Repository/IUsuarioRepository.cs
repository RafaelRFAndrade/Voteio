using Voteio.Entities;
using Voteio.Messaging.RawQuery;

namespace Voteio.Interfaces.Repository
{
    public interface IUsuarioRepository
    {
        void InserirUsuario(Usuario usuario);
        List<Usuario> ListarUsuarios();
        Usuario ObterPorEmail(string email);
        Usuario ObterPorId(string userId);
        List<ObterIdeiasVotadasRawQuery> ObterIdeiasVotadas(Guid codigoUsuario);
    }
}
