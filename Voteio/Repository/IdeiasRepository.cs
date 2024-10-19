using Microsoft.EntityFrameworkCore;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Repository.Base;

namespace Voteio.Repository
{
    public class IdeiasRepository : RepositoryBase, IIdeiasRepository
    {
        public IdeiasRepository(DbContextOptions<RepositoryBase> options) : base(options)
        {
        }

        public List<Ideias> ListarIdeias()
        {
            return Ideias.ToList();
        }

        public void InserirIdeia(Ideias ideia)
        {
            Add(ideia);
            SaveChanges();
        }
    }
}
