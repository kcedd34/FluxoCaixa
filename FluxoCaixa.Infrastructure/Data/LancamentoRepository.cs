using FluxoCaixa.Domain.Entities;
using FluxoCaixa.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoCaixa.Infrastructure.Data
{
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly FluxoCaixaDbContext _context;

        public LancamentoRepository(FluxoCaixaDbContext context)
        {
            _context = context;
        }

        public async Task<Lancamento> ObterPorIdAsync(Guid id)
        {
            return await _context.Lancamentos.FindAsync(id);
        }

        public async Task<IEnumerable<Lancamento>> ObterPorDataAsync(DateTime data)
        {
            return await _context.Lancamentos
                .Where(l => l.Data.Date == data.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lancamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _context.Lancamentos
                .Where(l => l.Data.Date >= inicio.Date && l.Data.Date <= fim.Date)
                .OrderBy(l => l.Data)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Lancamento lancamento)
        {
            await _context.Lancamentos.AddAsync(lancamento);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Lancamento lancamento)
        {
            _context.Lancamentos.Update(lancamento);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            var lancamento = await ObterPorIdAsync(id);
            if (lancamento != null)
            {
                _context.Lancamentos.Remove(lancamento);
                await _context.SaveChangesAsync();
            }
        }
    }
}