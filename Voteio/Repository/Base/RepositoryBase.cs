using Microsoft.EntityFrameworkCore;
using Voteio.Entities;

namespace Voteio.Repository.Base
{
    public class RepositoryBase : DbContext
    {
        public RepositoryBase(DbContextOptions<RepositoryBase> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
