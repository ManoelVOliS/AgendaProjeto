using Agenda.Core.Entities;

namespace Agenda.Core.Interface 
{
    public interface IContatoRepository
    {
        Task<Contato?> GetByIdAsync(int id);
        Task<IEnumerable<Contato>> GetAllAsync();
        Task<Contato> AddAsync(Contato contato);
        Task UpdateAsync(Contato contato);
        Task DeleteAsync(Contato contato);
        Task<Contato?> GetByEmailAsync(string email);
        Task<Contato?> GetByTelefoneAsync(string telefone);
    }
}