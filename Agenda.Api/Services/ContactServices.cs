using Agenda.Core.Entities;
using Agenda.Core.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agenda.Api.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<Contact> CreateContactAsync(Contact contact)
        {
            var infoExists = await _contactRepository.GetByEmailAsync(contact.Email);
            if(infoExists != null)
            {
                throw new Exception("Email já cadastrado");
            }

            var phoneExists = await _contactRepository.GetByPhoneAsync(contact.Phone);
            if(phoneExists != null)
            {
                throw new Exception("Telefone já cadastrado");
            }

            return await _contactRepository.AddAsync(contact);
        }

        public async Task DeleteContactAsync(int id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact != null)
            {
                await _contactRepository.DeleteAsync(contact);
            }
        }
        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return await _contactRepository.GetAllAsync();
        }

        public async Task<Contact?> GetContactByIdAsync(int id)
        {
            return await _contactRepository.GetByIdAsync(id);
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            var emailExists = await _contactRepository.GetByEmailAsync(contact.Email);
            if (emailExists != null && emailExists.Id != contact.Id) 
            {
                throw new System.Exception("Email já cadastrado");
            }
            
            var phoneExists = await _contactRepository.GetByPhoneAsync(contact.Phone);
            if (phoneExists != null && phoneExists.Id != contact.Id)
            {
                throw new System.Exception("Telefone já cadastrado");
            }
            var contactExists = await _contactRepository.GetByIdAsync(contact.Id);
            if (contactExists == null)
            {
                throw new System.Exception("Contact não encontrado");
            }

            contactExists.Name = contact.Name;
            contactExists.Email = contact.Email;
            contactExists.Phone = contact.Phone;

            await _contactRepository.UpdateAsync(contactExists);
        }
    }
}