using System;
using System.Threading.Tasks;

namespace FluxoCaixa.Application.Interfaces
{
    public interface ICacheService
    {
        Task<T> ObterAsync<T>(string chave) where T : class;
        Task ArmazenarAsync<T>(string chave, T valor, TimeSpan? expiracao = null) where T : class;
        Task RemoverAsync(string chave);
    }
}