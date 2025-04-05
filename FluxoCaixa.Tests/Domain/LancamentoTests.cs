using FluxoCaixa.Domain.Entities;
using System;
using Xunit;

namespace FluxoCaixa.Tests.Domain
{
    public class LancamentoTests
    {
        [Fact]
        public void CriarLancamento_ComValorNegativo_DeveLancarExcecao()
        {
            // Arrange
            var data = DateTime.Today;
            var descricao = "Teste";
            var valor = -100m;
            var tipo = TipoLancamento.Debito;
            var categoria = "Geral";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Lancamento(data, descricao, valor, tipo, categoria));
        }

        [Fact]
        public void CriarLancamento_ComDadosValidos_DeveCriarCorretamente()
        {
            // Arrange
            var data = DateTime.Today;
            var descricao = "Teste";
            var valor = 100m;
            var tipo = TipoLancamento.Credito;
            var categoria = "Vendas";

            // Act
            var lancamento = new Lancamento(data, descricao, valor, tipo, categoria);

            // Assert
            Assert.Equal(data.Date, lancamento.Data);
            Assert.Equal(descricao, lancamento.Descricao);
            Assert.Equal(valor, lancamento.Valor);
            Assert.Equal(tipo, lancamento.Tipo);
            Assert.Equal(categoria, lancamento.Categoria);
            Assert.NotEqual(Guid.Empty, lancamento.Id);
        }
    }
}