using System;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(T entity);
        Task AtualizarAsync(T entity);
        Task RemoverAsync(Guid id);
    }
}