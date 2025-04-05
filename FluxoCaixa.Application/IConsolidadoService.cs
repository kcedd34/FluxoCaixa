using FluxoCaixa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoCaixa.Application.Interfaces
{
    public interface IConsolidadoService
    {
        Task<ConsolidadoDiarioDto> ObterConsolidadoPorDataAsync(DateTime data);
        Task<IEnumerable<ConsolidadoDiarioDto>> ObterConsolidadoPeriodoAsync(DateTime inicio, DateTime fim);
        Task ProcessarLancamentoAsync(Guid lancamentoId, DateTime data, decimal valor, string tipo);
    }
}