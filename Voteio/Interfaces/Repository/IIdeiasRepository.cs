using Voteio.Entities;
using Voteio.Messaging.RawQuery;
using Voteio.Messaging.Requests;

namespace Voteio.Interfaces.Repository
{
    public interface IIdeiasRepository
    {
        void InserirIdeia(Ideias ideia);
        List<ObterIdeiasRawQuery> ObterIdeias(ObterIdeiasRequest obterIdeiasRequest);
    }
}
