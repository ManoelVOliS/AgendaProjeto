using Agenda.Core.Entities;
using Agenda.Core.Interface;
using Agenda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AgendaDbContext _dbContext;

        public ContactRepository(AgendaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Contact> AddAsync(Contact contact)
        {
            await _dbContext.Contacts.AddAsync(contact);
            await _dbContext.SaveChangesAsync();
            return contact;
        }

        public async Task DeleteAsync(Contact contact)
        {
            _dbContext.Contacts.Remove(contact);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _dbContext.Contacts.ToListAsync();
        }

        public async Task<Contact?> GetByIdAsync(int id)
        {
            return await _dbContext.Contacts.FindAsync(id);
        }

        public async Task<Contact?> GetByEmailAsync(string email)
        {
            return await _dbContext.Contacts
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<Contact?> GetByPhoneAsync(string Phone)
        {
            return await _dbContext.Contacts
                .FirstOrDefaultAsync(c => c.Phone == Phone.Trim());
        }

        public async Task UpdateAsync(Contact contact)
        {
            _dbContext.Contacts.Update(contact);
            await _dbContext.SaveChangesAsync();
        }
    }
}