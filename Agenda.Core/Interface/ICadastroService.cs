using Agenda.Core.Entities;

namespace Agenda.Core.Interface
{
    public interface ICadastroService
    {
        Task<Contato?> GetContatoByIdAsync(int id);
        Task<IEnumerable<Contato>> GetAllContatosAsync();
        Task<Contato> CreateContatoAsync(Contato contato);
        Task UpdateContatoAsync(Contato contato);
        Task DeleteContatoAsync(int id);
    
    }
}