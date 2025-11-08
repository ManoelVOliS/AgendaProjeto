using Agenda.Core.Entities;
using Agenda.Core.Interface; 
using Microsoft.AspNetCore.Mvc;
using Agenda.Core.Dtos;
using AutoMapper; 

namespace Agenda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContatosController : ControllerBase
    {
        private readonly ICadastroService _cadastroService;
        private readonly IMapper _mapper; // O campo (field)

        public ContatosController(ICadastroService cadastroService, IMapper mapper)
        {
            _cadastroService = cadastroService;
            _mapper = mapper; // Agora "mapper" (o parâmetro) é atribuído a "_mapper" (o campo)
        }

        // GET: api/Contatos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contatos = await _cadastroService.GetAllContatosAsync();
            return Ok(contatos);
        }

        // GET: api/Contatos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contato = await _cadastroService.GetContatoByIdAsync(id);

            if (contato == null)
            {
                return NotFound();
            }

            return Ok(contato);
        }

        // POST: api/Contatos
        [HttpPost]
        public async Task<IActionResult> Create(CreateContatoDto contatoDto)
        {
        
            var contato = _mapper.Map<Contato>(contatoDto); 
            
            var novoContato = await _cadastroService.CreateContatoAsync(contato);

            return CreatedAtAction(nameof(GetById), new { id = novoContato.Id }, novoContato);
        }

        // PUT: api/Contatos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateContatoDto contatoDto)
        {
            var contato = _mapper.Map<Contato>(contatoDto);
            contato.Id = id; // Define o ID a partir da URL

            try
            {
                await _cadastroService.UpdateContatoAsync(contato);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent(); // Sucesso
        }

        // DELETE: api/Contatos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cadastroService.DeleteContatoAsync(id);
            return NoContent();
        }
    }
}