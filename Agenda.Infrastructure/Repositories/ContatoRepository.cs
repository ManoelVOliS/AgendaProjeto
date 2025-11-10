using Agenda.Core.Entities;
using Agenda.Core.Interface;
using Agenda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Repositories
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly AgendaDbContext _dbContext;

        public ContatoRepository(AgendaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Contato> AddAsync(Contato contato)
        {
            await _dbContext.Contatos.AddAsync(contato);
            await _dbContext.SaveChangesAsync();
            return contato;
        }

        public async Task DeleteAsync(Contato contato)
        {
            _dbContext.Contatos.Remove(contato);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contato>> GetAllAsync()
        {
            return await _dbContext.Contatos.ToListAsync();
        }

        public async Task<Contato?> GetByIdAsync(int id)
        {
            return await _dbContext.Contatos.FindAsync(id);
        }

        public async Task<Contato?> GetByEmailAsync(string email)
        {
            return await _dbContext.Contatos
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<Contato?> GetByTelefoneAsync(string telefone)
        {
            return await _dbContext.Contatos
                .FirstOrDefaultAsync(c => c.Telefone == telefone.Trim());
        }

        public async Task UpdateAsync(Contato contato)
        {
            _dbContext.Contatos.Update(contato);
            await _dbContext.SaveChangesAsync();
        }
    }
}