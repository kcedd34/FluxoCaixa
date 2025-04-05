using FluxoCaixa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Interfaces
{
    public interface ILancamentoRepository : IRepository<Lancamento>
    {
        Task<IEnumerable<Lancamento>> ObterPorDataAsync(DateTime data);
        Task<IEnumerable<Lancamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
    }
}