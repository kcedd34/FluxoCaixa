using FluxoCaixa.Application.DTOs;
using FluxoCaixa.Application.Interfaces;
using FluxoCaixa.Application.Services;
using FluxoCaixa.Domain.Entities;
using FluxoCaixa.Domain.Events;
using FluxoCaixa.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FluxoCaixa.Tests.Application
{
    public class LancamentoServiceTests
    {
        private readonly Mock<ILancamentoRepository> _repositoryMock;
        private readonly Mock<IEventBus> _eventBusMock;
        private readonly ILancamentoService _service;

        public LancamentoServiceTests()
        {
            _repositoryMock = new Mock<ILancamentoRepository>();
            _eventBusMock = new Mock<IEventBus>();
            _service = new LancamentoService(_repositoryMock.Object, _eventBusMock.Object);
        }

        [Fact]
        public async Task CriarLancamentoAsync_ComDadosValidos_DeveCriarEPublicarEvento()
        {
            // Arrange
            var dto = new LancamentoDto
            {
                Data = DateTime.Today,
                Descricao = "Teste",
                Valor = 100m,
                Tipo = "Credito",
                Categoria = "Vendas"
            };

            _repositoryMock.Setup(r => r.AdicionarAsync(It.IsAny<Lancamento>()))
                .Returns(Task.CompletedTask);

            _eventBusMock.Setup(e => e.PublicarAsync(It.IsAny<LancamentoCriadoEvent>()))
                .Returns(Task.CompletedTask);

            // Act
            var id = await _service.CriarLancamentoAsync(dto);

            // Assert
            Assert.NotEqual(Guid.Empty, id);
            _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Lancamento>()), Times.Once);
            _eventBusMock.Verify(e => e.PublicarAsync(It.IsAny<LancamentoCriadoEvent>()), Times.Once);
        }

        [Fact]
        public async Task ObterLancamentosPorDataAsync_DeveRetornarListaCorreta()
        {
            // Arrange
            var data = DateTime.Today;
            var lancamentos = new List<Lancamento>
            {
                new Lancamento(data, "Teste 1", 100m, TipoLancamento.Credito, "Vendas"),
                new Lancamento(data, "Teste 2", 50m, TipoLancamento.Debito, "Despesas")
            };

            _repositoryMock.Setup(r => r.ObterPorDataAsync(data))
                .ReturnsAsync(lancamentos);

            // Act
            var result = await _service.ObterLancamentosPorDataAsync(data);

            // Assert
            Assert.Equal(2, result.Count());
            _repositoryMock.Verify(r => r.ObterPorDataAsync(data), Times.Once);
        }
    }
}