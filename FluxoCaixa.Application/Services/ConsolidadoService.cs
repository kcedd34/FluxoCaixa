using FluxoCaixa.Application.DTOs;
using FluxoCaixa.Application.Interfaces;
using FluxoCaixa.Domain.Entities;
using FluxoCaixa.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoCaixa.Application.Services
{
    public class ConsolidadoService : IConsolidadoService
    {
        private readonly IConsolidadoRepository _repository;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ICacheService _cacheService;

        public ConsolidadoService(
            IConsolidadoRepository repository,
            ILancamentoRepository lancamentoRepository,
            ICacheService cacheService)
        {
            _repository = repository;
            _lancamentoRepository = lancamentoRepository;
            _cacheService = cacheService;
        }

        public async Task<ConsolidadoDiarioDto> ObterConsolidadoPorDataAsync(DateTime data)
        {
            // Tenta obter do cache primeiro
            var cacheKey = $"consolidado:{data:yyyy-MM-dd}";
            var consolidadoCache = await _cacheService.ObterAsync<ConsolidadoDiarioDto>(cacheKey);

            if (consolidadoCache != null)
                return consolidadoCache;

            // Se não estiver em cache, busca do repositório
            var consolidado = await _repository.ObterPorDataAsync(data);

            if (consolidado == null)
            {
                // Se não existir, calcula sob demanda
                await ProcessarConsolidacaoDiaAsync(data);
                consolidado = await _repository.ObterPorDataAsync(data);

                // Se ainda for nulo, retorna um consolidado vazio
                if (consolidado == null)
                {
                    return new ConsolidadoDiarioDto
                    {
                        Data = data,
                        TotalCreditos = 0,
                        TotalDebitos = 0,
                        SaldoDia = 0,
                        SaldoAcumulado = 0
                    };
                }
            }

            // Converte para DTO
            var dto = new ConsolidadoDiarioDto
            {
                Data = consolidado.Data,
                TotalCreditos = consolidado.TotalCreditos,
                TotalDebitos = consolidado.TotalDebitos,
                SaldoDia = consolidado.SaldoDia,
                SaldoAcumulado = consolidado.SaldoAcumulado
            };

            // Adiciona ao cache com expiração
            await _cacheService.ArmazenarAsync(cacheKey, dto, TimeSpan.FromMinutes(15));

            return dto;
        }

        public async Task<IEnumerable<ConsolidadoDiarioDto>> ObterConsolidadoPeriodoAsync(DateTime inicio, DateTime fim)
        {
            var consolidados = await _repository.ObterPorPeriodoAsync(inicio, fim);
            return consolidados.Select(c => new ConsolidadoDiarioDto
            {
                Data = c.Data,
                TotalCreditos = c.TotalCreditos,
                TotalDebitos = c.TotalDebitos,
                SaldoDia = c.SaldoDia,
                SaldoAcumulado = c.SaldoAcumulado
            });
        }

        public async Task ProcessarLancamentoAsync(Guid lancamentoId, DateTime data, decimal valor, string tipo)
        {
            // Invalida o cache
            var cacheKey = $"consolidado:{data:yyyy-MM-dd}";
            await _cacheService.RemoverAsync(cacheKey);

            // Processa o consolidado
            await ProcessarConsolidacaoDiaAsync(data);
        }

        private async Task ProcessarConsolidacaoDiaAsync(DateTime data)
        {
            // Obtém todos os lançamentos do dia
            var lancamentos = await _lancamentoRepository.ObterPorDataAsync(data);

            // Calcula totais
            var totalCreditos = lancamentos
                .Where(l => l.Tipo == TipoLancamento.Credito)
                .Sum(l => l.Valor);

            var totalDebitos = lancamentos
                .Where(l => l.Tipo == TipoLancamento.Debito)
                .Sum(l => l.Valor);

            // Obtém consolidado anterior para saldo acumulado
            var diaAnterior = data.AddDays(-1);
            var consolidadoAnterior = await _repository.ObterPorDataAsync(diaAnterior);
            var saldoAnterior = consolidadoAnterior?.SaldoAcumulado ?? 0;

            // Verifica se já existe consolidado para o dia
            var consolidadoExistente = await _repository.ObterPorDataAsync(data);

            if (consolidadoExistente != null)
            {
                // Atualiza
                consolidadoExistente.Atualizar(totalCreditos, totalDebitos, saldoAnterior);
                await _repository.AtualizarAsync(consolidadoExistente);
            }
            else
            {
                // Cria novo
                var consolidado = new ConsolidadoDiario(data, totalCreditos, totalDebitos, saldoAnterior);
                await _repository.AdicionarAsync(consolidado);
            }
        }
    }
}