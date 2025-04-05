using FluxoCaixa.Domain.Entities;
using FluxoCaixa.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoCaixa.Infrastructure.Data
{
    public class ConsolidadoRepository : IConsolidadoRepository
    {
        private readonly FluxoCaixaDbContext _context;

        public ConsolidadoRepository(FluxoCaixaDbContext context)
        {
            _context = context;
        }

        public async Task<ConsolidadoDiario> ObterPorIdAsync(Guid id)
        {
            return await _context.Consolidados.FindAsync(id);
        }

        public async Task<ConsolidadoDiario> ObterPorDataAsync(DateTime data)
        {
            return await _context.Consolidados
                .FirstOrDefaultAsync(c => c.Data.Date == data.Date);
        }

        public async Task<IEnumerable<ConsolidadoDiario>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _context.Consolidados
                .Where(c => c.Data.Date >= inicio.Date && c.Data.Date <= fim.Date)
                .OrderBy(c => c.Data)
                .ToListAsync();
        }

        public async Task AdicionarAsync(ConsolidadoDiario consolidado)
        {
            await _context.Consolidados.AddAsync(consolidado);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(ConsolidadoDiario consolidado)
        {
            _context.Consolidados.Update(consolidado);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            var consolidado = await ObterPorIdAsync(id);
            if (consolidado != null)
            {
                _context.Consolidados.Remove(consolidado);
                await _context.SaveChangesAsync();
            }
        }
    }
}