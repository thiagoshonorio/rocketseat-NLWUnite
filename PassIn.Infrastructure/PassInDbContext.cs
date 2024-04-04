using Microsoft.EntityFrameworkCore;
using PassIn.Infrastructure.Entitites;

namespace PassIn.Infrastructure
{
    public class PassInDbContext : DbContext //Converter a "tradução" de querys
    {
        //Representa a tabela eventos
        public DbSet<Event> Events { get; set; }

        //configurar o contexto com a base de dados
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=D:\\Desenvolvimento\\C#\\Repos\\rocketseat\\PassInDb.db");
        }
    }
}
