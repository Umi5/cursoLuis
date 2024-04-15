using System.Reflection;
using Curso.Bussiness.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Curso.Bussiness.Data.DbContexts
{
    public class CursoDbContext : DbContext
    {
        public CursoDbContext(DbContextOptions<CursoDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
