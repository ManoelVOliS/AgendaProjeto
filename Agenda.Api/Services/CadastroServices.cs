using Agenda.Core.Entities;
using Agenda.Core.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agenda.Api.Services
{
    public class CadastroService : ICadastroService
    {
        private readonly IContatoRepository _contatoRepository;

        public CadastroService(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public async Task<Contato> CreateContatoAsync(Contato contato)
        {
            var infoExistente = await _contatoRepository.GetByEmailAsync(contato.Email);
            if(infoExistente != null)
            {
                throw new Exception("Email já cadastrado");
            }

            var telefoneExistente = await _contatoRepository.GetByTelefoneAsync(contato.Telefone);
            if(telefoneExistente != null)
            {
                throw new Exception("Telefone já cadastrado");
            }
            
            return await _contatoRepository.AddAsync(contato);
        }

        public async Task DeleteContatoAsync(int id)
        {
            var contato = await _contatoRepository.GetByIdAsync(id);
            if (contato != null)
            {
                await _contatoRepository.DeleteAsync(contato);
            }
        }
        public async Task<IEnumerable<Contato>> GetAllContatosAsync()
        {
            return await _contatoRepository.GetAllAsync();
        }

        public async Task<Contato?> GetContatoByIdAsync(int id)
        {
            return await _contatoRepository.GetByIdAsync(id);
        }

        public async Task UpdateContatoAsync(Contato contato)
        {
            var contatoExistente = await _contatoRepository.GetByIdAsync(contato.Id);
            if (contatoExistente == null)
            {
                throw new System.Exception("Contato não encontrado");
            }

            contatoExistente.Nome = contato.Nome;
            contatoExistente.Email = contato.Email;
            contatoExistente.Telefone = contato.Telefone;

            await _contatoRepository.UpdateAsync(contatoExistente);
        }
    }
}