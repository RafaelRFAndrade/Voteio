using Voteio.Entities;
using Voteio.Messaging.RawQuery;

namespace Voteio.Interfaces.Repository
{
    public interface IIdeiasRepository
    {
        void InserirIdeia(Ideias ideia);
        List<ObterIdeiasRawQuery> ObterIdeias();
    }
}
