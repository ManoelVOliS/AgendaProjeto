using Agenda.Core.Entities;

namespace Agenda.Core.Interface
{
    public interface IContactService
    {
        Task<Contact?> GetContactByIdAsync(int id);
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> CreateContactAsync(Contact contact);
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(int id);
    
    }
}