using Agenda.Core.Entities;
using Agenda.Core.Interface; 
using Microsoft.AspNetCore.Mvc;
using Agenda.Core.Dtos;

namespace Agenda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contactsDto = await _contactService.GetAllContactsAsync();
            return Ok(contactsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contactDto = await _contactService.GetContactByIdAsync(id);

            if (contactDto == null)
            {
                return NotFound();
            }

            return Ok(contactDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateContactDto contactDto)
        {
            try
            {
                var newContactDto = await _contactService.CreateContactAsync(contactDto);

                return CreatedAtAction(nameof(GetById), new { id = newContactDto.Id }, newContactDto);    
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateContactDto contactDto)
        { 
            try
            {
                await _contactService.UpdateContactAsync(id, contactDto);
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