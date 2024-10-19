using Voteio.Entities;
using Voteio.Messaging.Responses;

namespace Voteio.Interfaces.Repository
{
    public interface IComentarioRepository
    {
        void InserirComentario(Comentario comentario);
        List<ComentarioDto> ObterComentariosPorIdeia(Guid codigoIdeia);
    }
}
