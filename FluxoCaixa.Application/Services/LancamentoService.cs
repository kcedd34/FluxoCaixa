using FluxoCaixa.Application.DTOs;
using FluxoCaixa.Application.Interfaces;
using FluxoCaixa.Domain.Entities;
using FluxoCaixa.Domain.Events;
using FluxoCaixa.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoCaixa.Application.Services
{
    public class LancamentoService : ILancamentoService
    {
        private readonly ILancamentoRepository _repository;
        private readonly IEventBus _eventBus;

        public LancamentoService(ILancamentoRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public async Task<Guid> CriarLancamentoAsync(LancamentoDto dto)
        {
            // Validações
            if (dto.Valor <= 0)
                throw new ArgumentException("Valor deve ser positivo");

            // Converte tipo de string para enum
            var tipoLancamento = Enum.Parse<TipoLancamento>(dto.Tipo, true);

            // Cria entidade
            var lancamento = new Lancamento(dto.Data, dto.Descricao, dto.Valor, tipoLancamento, dto.Categoria);

            // Persiste
            await _repository.AdicionarAsync(lancamento);

            // Publica evento para processamento assíncrono
            await _eventBus.PublicarAsync(new LancamentoCriadoEvent(lancamento.Id, lancamento.Data,
                                                                 lancamento.Valor, lancamento.Tipo));

            return lancamento.Id;
        }

        public async Task<LancamentoDto> ObterLancamentoPorIdAsync(Guid id)
        {
            var lancamento = await _repository.ObterPorIdAsync(id);
            if (lancamento == null)
                return null;

            return new LancamentoDto
            {
                Id = lancamento.Id,
                Data = lancamento.Data,
                Descricao = lancamento.Descricao,
                Valor = lancamento.Valor,
                Tipo = lancamento.Tipo.ToString(),
                Categoria = lancamento.Categoria
            };
        }

        public async Task<bool> AtualizarLancamentoAsync(Guid id, LancamentoDto dto)
        {
            var lancamento = await _repository.ObterPorIdAsync(id);
            if (lancamento == null)
                return false;

            // Validações
            if (dto.Valor <= 0)
                throw new ArgumentException("Valor deve ser positivo");

            // Converte tipo de string para enum
            var tipoLancamento = Enum.Parse<TipoLancamento>(dto.Tipo, true);

            // Atualiza entidade
            lancamento.Atualizar(dto.Descricao, dto.Valor, tipoLancamento, dto.Categoria);

            // Persiste
            await _repository.AtualizarAsync(lancamento);

            // Publica evento para processamento assíncrono
            await _eventBus.PublicarAsync(new LancamentoCriadoEvent(lancamento.Id, lancamento.Data,
                                                                 lancamento.Valor, lancamento.Tipo));

            return true;
        }

        public async Task<bool> RemoverLancamentoAsync(Guid id)
        {
            var lancamento = await _repository.ObterPorIdAsync(id);
            if (lancamento == null)
                return false;

            await _repository.RemoverAsync(id);
            return true;
        }

        public async Task<IEnumerable<LancamentoDto>> ObterLancamentosPorDataAsync(DateTime data)
        {
            var lancamentos = await _repository.ObterPorDataAsync(data);
            return lancamentos.Select(l => new LancamentoDto
            {
                Id = l.Id,
                Data = l.Data,
                Descricao = l.Descricao,
                Valor = l.Valor,
                Tipo = l.Tipo.ToString(),
                Categoria = l.Categoria
            });
        }

        public async Task<IEnumerable<LancamentoDto>> ObterLancamentosPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            var lancamentos = await _repository.ObterPorPeriodoAsync(inicio, fim);
            return lancamentos.Select(l => new LancamentoDto
            {
                Id = l.Id,
                Data = l.Data,
                Descricao = l.Descricao,
                Valor = l.Valor,
                Tipo = l.Tipo.ToString(),
                Categoria = l.Categoria
            });
        }
    }
}