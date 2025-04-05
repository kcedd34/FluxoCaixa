using System;

namespace FluxoCaixa.Domain.Entities
{
    public class ConsolidadoDiario
    {
        public Guid Id { get; private set; }
        public DateTime Data { get; private set; }
        public decimal TotalCreditos { get; private set; }
        public decimal TotalDebitos { get; private set; }
        public decimal SaldoDia => TotalCreditos - TotalDebitos;
        public decimal SaldoAcumulado { get; private set; }
        public DateTime UltimaAtualizacao { get; private set; }

        // Construtor privado para ORM
        private ConsolidadoDiario() { }

        // Construtor para criar um novo consolidado
        public ConsolidadoDiario(DateTime data, decimal totalCreditos, decimal totalDebitos, decimal saldoAnterior)
        {
            Id = Guid.NewGuid();
            Data = data.Date; // Apenas a data, sem hora
            TotalCreditos = totalCreditos;
            TotalDebitos = totalDebitos;
            SaldoAcumulado = saldoAnterior + SaldoDia;
            UltimaAtualizacao = DateTime.UtcNow;
        }

        // Método para atualização
        public void Atualizar(decimal totalCreditos, decimal totalDebitos, decimal saldoAnterior)
        {
            TotalCreditos = totalCreditos;
            TotalDebitos = totalDebitos;
            SaldoAcumulado = saldoAnterior + SaldoDia;
            UltimaAtualizacao = DateTime.UtcNow;
        }
    }
}