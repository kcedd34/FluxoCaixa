using System;

namespace FluxoCaixa.Application.DTOs
{
    public class ConsolidadoDiarioDto
    {
        public DateTime Data { get; set; }
        public decimal TotalCreditos { get; set; }
        public decimal TotalDebitos { get; set; }
        public decimal SaldoDia { get; set; }
        public decimal SaldoAcumulado { get; set; }
    }
}