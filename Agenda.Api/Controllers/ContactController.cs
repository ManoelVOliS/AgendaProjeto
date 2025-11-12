using Agenda.Core.Entities;
using Agenda.Core.Interface; 
using Microsoft.AspNetCore.Mvc;
using Agenda.Core.Dtos;
using AutoMapper; 

namespace Agenda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper; 

        public ContactsController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateContactDto contactDto)
        {
            try
            {
                var contact = _mapper.Map<Contact>(contactDto); 
                var novoContact = await _contactService.CreateContactAsync(contact);

                return CreatedAtAction(nameof(GetById), new { id = novoContact.Id }, novoContact);    
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateContactDto contactDto)
        {
            var contact = _mapper.Map<Contact>(contactDto);
            contact.Id = id; 

            try
            {
                await _contactService.UpdateContactAsync(contact);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contactService.DeleteContactAsync(id);
            return NoContent();
        }
    }
}