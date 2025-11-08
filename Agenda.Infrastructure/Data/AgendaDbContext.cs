using Agenda.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Data
{
    public class AgendaDbContext : DbContext
    {
        public AgendaDbContext(DbContextOptions<AgendaDbContext> options) : base(options)
        {
        }
        public DbSet<Contato> Contatos { get; set; }
    }
}