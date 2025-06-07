using InvestmentControlApi.Application.Services;
using InvestmentControlApi.Domain.Entities;
using InvestmentControlApi.Domain.Enums;
using Xunit;

namespace InvestmentControlApi.Tests.Services
{
    public class PrecoMedioServiceTests
    {
        private readonly PrecoMedioService _service = new();

        [Fact]
        public void CalcularPrecoMedio_DeveCalcularCorretamenteComValoresValidos()
        {
            // Arrange
            var operacoes = new List<Operacao>
            {
                new() { TipoOperacao = TipoOperacao.Compra, Quantidade = 10, PrecoUnitario = 20 },
                new() { TipoOperacao = TipoOperacao.Compra, Quantidade = 5, PrecoUnitario = 40 }
            };

            // Act
            var resultado = _service.CalcularPrecoMedio(operacoes);

            // Assert
            Assert.Equal(26.67m, resultado);
        }

        [Fact]
        public void CalcularPrecoMedio_DeveLancarExcecao_ListaVazia()
        {
            // Arrange
            var operacoes = new List<Operacao>();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _service.CalcularPrecoMedio(operacoes));
            Assert.Contains("vazia", ex.Message.ToLower());
        }

        [Fact]
        public void CalcularPrecoMedio_DeveLancarExcecao_SemCompras()
        {
            // Arrange
            var operacoes = new List<Operacao>
            {
                new() { TipoOperacao = TipoOperacao.Venda, Quantidade = 10, PrecoUnitario = 20 }
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _service.CalcularPrecoMedio(operacoes));
            Assert.Contains("compra", ex.Message.ToLower());
        }

        [Fact]
        public void CalcularPrecoMedio_DeveLancarExcecao_QuantidadeZero()
        {
            // Arrange
            var operacoes = new List<Operacao>
            {
                new() { TipoOperacao = TipoOperacao.Compra, Quantidade = 0, PrecoUnitario = 50 }
            };

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _service.CalcularPrecoMedio(operacoes));
            Assert.Contains("quantidade", ex.Message.ToLower());
        }
    }
}