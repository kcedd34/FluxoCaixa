using System;

namespace FluxoCaixa.Application.DTOs
{
    public class LancamentoDto
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; }
        public string Categoria { get; set; }
    }
}