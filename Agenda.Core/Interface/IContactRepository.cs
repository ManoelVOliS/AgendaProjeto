using Agenda.Core.Entities;

namespace Agenda.Core.Interface 
{
    public interface IContactRepository
    {
        Task<Contact?> GetByIdAsync(int id);
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact> AddAsync(Contact contact);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(Contact contact);
        Task<Contact?> GetByEmailAsync(string email);
        Task<Contact?> GetByPhoneAsync(string phone);
    }
}