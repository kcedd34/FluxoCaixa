using FluxoCaixa.Domain.Entities;
using System;

namespace FluxoCaixa.Domain.Events
{
    public class LancamentoCriadoEvent : DomainEvent
    {
        public Guid LancamentoId { get; }
        public DateTime Data { get; }
        public decimal Valor { get; }
        public TipoLancamento Tipo { get; }

        public LancamentoCriadoEvent(Guid lancamentoId, DateTime data, decimal valor, TipoLancamento tipo)
            : base()
        {
            LancamentoId = lancamentoId;
            Data = data;
            Valor = valor;
            Tipo = tipo;
        }
    }
}