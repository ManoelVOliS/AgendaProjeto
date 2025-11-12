using Agenda.Core.Dtos;
using Agenda.Core.Entities;

namespace Agenda.Core.Interface
{
    public interface IContactService
    {
        Task<ContactResponseDto?> GetContactByIdAsync(int id);
        Task<IEnumerable<ContactResponseDto>> GetAllContactsAsync();
        Task<ContactResponseDto> CreateContactAsync(CreateContactDto createDto);
        Task UpdateContactAsync(int id, UpdateContactDto updateDto);
        Task DeleteContactAsync(int id);
    
    }
}