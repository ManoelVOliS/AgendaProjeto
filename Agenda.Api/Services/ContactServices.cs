using Agenda.Core.Entities;
using Agenda.Core.Interface;
using Agenda.Core.Dtos;
using AutoMapper;
using Agenda.Core.Exceptions;

namespace Agenda.Api.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContactResponseDto>> GetAllContactsAsync()
        {
            var contacts = await _contactRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ContactResponseDto>>(contacts);
        }

        public async Task<ContactResponseDto?> GetContactByIdAsync(int id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);

            return _mapper.Map<ContactResponseDto?>(contact);
        }
        
        public async Task<ContactResponseDto> CreateContactAsync(CreateContactDto createDto)
        {
            var contact = _mapper.Map<Contact>(createDto);
            var infoExists = await _contactRepository.GetByEmailAsync(contact.Email);

            if (infoExists != null)
            {
                throw new DuplicateDataException("Email já cadastrado");
            }

            var phoneExists = await _contactRepository.GetByPhoneAsync(contact.Phone);
            if (phoneExists != null)
            {
                throw new DuplicateDataException("Telefone já cadastrado");
            }

            var newContact = await _contactRepository.AddAsync(contact);
            
            return _mapper.Map<ContactResponseDto>(newContact);
        }

        public async Task DeleteContactAsync(int id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact != null)
            {
                await _contactRepository.DeleteAsync(contact);
            }
        }

        public async Task UpdateContactAsync(int id, UpdateContactDto updateDto)
        {
            var contact = _mapper.Map<Contact>(updateDto);
            var emailExists = await _contactRepository.GetByEmailAsync(contact.Email);
            Console.WriteLine(id);
            if (emailExists != null && emailExists.Id != id)
            {
                throw new DuplicateDataException("Email já cadastrado");
            }

            var phoneExists = await _contactRepository.GetByPhoneAsync(contact.Phone);
            if (phoneExists != null && phoneExists.Id != id)
            {
                throw new DuplicateDataException("Telefone já cadastrado");
            }
            var contactExists = await _contactRepository.GetByIdAsync(id);
            if (contactExists == null)
            {
                throw new NotFoundException("Contact não encontrado");
            }

            contactExists.Name = contact.Name;
            contactExists.Email = contact.Email;
            contactExists.Phone = contact.Phone;

            await _contactRepository.UpdateAsync(contactExists);
        }
    }
}