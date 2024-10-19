using Voteio.Entities;

namespace Voteio.Interfaces.Repository
{
    public interface IIdeiasRepository
    {
        List<Ideias> ListarIdeias();
        void InserirIdeia(Ideias ideia);
    }
}
