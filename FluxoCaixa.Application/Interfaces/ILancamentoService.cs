using FluxoCaixa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoCaixa.Application.Interfaces
{
    public interface ILancamentoService
    {
        Task<Guid> CriarLancamentoAsync(LancamentoDto lancamento);
        Task<LancamentoDto> ObterLancamentoPorIdAsync(Guid id);
        Task<bool> AtualizarLancamentoAsync(Guid id, LancamentoDto lancamento);
        Task<bool> RemoverLancamentoAsync(Guid id);
        Task<IEnumerable<LancamentoDto>> ObterLancamentosPorDataAsync(DateTime data);
        Task<IEnumerable<LancamentoDto>> ObterLancamentosPorPeriodoAsync(DateTime inicio, DateTime fim);
    }
}