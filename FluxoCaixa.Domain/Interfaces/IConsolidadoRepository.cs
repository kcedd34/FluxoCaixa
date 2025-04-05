using FluxoCaixa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Interfaces
{
    public interface IConsolidadoRepository : IRepository<ConsolidadoDiario>
    {
        Task<ConsolidadoDiario> ObterPorDataAsync(DateTime data);
        Task<IEnumerable<ConsolidadoDiario>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
    }
}