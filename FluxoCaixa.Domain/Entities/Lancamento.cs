using System;

namespace FluxoCaixa.Domain.Entities
{
    public class Lancamento
    {
        public Guid Id { get; private set; }
        public DateTime Data { get; private set; }
        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }
        public TipoLancamento Tipo { get; private set; }
        public string Categoria { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime? AtualizadoEm { get; private set; }

        // Construtor privado para ORM
        private Lancamento() { }

        // Construtor para criar um novo lançamento
        public Lancamento(DateTime data, string descricao, decimal valor,
                         TipoLancamento tipo, string categoria)
        {
            Id = Guid.NewGuid();
            Data = data.Date; // Apenas a data, sem hora
            Descricao = descricao;
            Valor = valor > 0 ? valor : throw new ArgumentException("Valor deve ser positivo");
            Tipo = tipo;
            Categoria = categoria;
            CriadoEm = DateTime.UtcNow;
        }

        // Método para atualização
        public void Atualizar(string descricao, decimal valor, TipoLancamento tipo, string categoria)
        {
            Descricao = descricao;
            Valor = valor > 0 ? valor : throw new ArgumentException("Valor deve ser positivo");
            Tipo = tipo;
            Categoria = categoria;
            AtualizadoEm = DateTime.UtcNow;
        }
    }

    public enum TipoLancamento
    {
        Debito = 0,
        Credito = 1
    }
}