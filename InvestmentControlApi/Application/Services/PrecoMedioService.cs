using InvestmentControlApi.Application.Interfaces;
using InvestmentControlApi.Domain.Entities;
using InvestmentControlApi.Domain.Enums;

namespace InvestmentControlApi.Application.Services
{
    public class PrecoMedioService : IPrecoMedioService
    {
        public decimal CalcularPrecoMedio(List<Operacao> operacoes)
        {
            if (operacoes == null || !operacoes.Any())
                throw new ArgumentException("Lista de operações inválida.");

            var compras = operacoes.Where(o => o.TipoOperacao == TipoOperacao.Compra).ToList();
            if (!compras.Any())
                throw new ArgumentException("Não há operações de compra.");

            var totalQuantidade = compras.Sum(o => o.Quantidade);
            if (totalQuantidade == 0)
                throw new InvalidOperationException("Quantidade total igual a zero.");

            var totalValor = compras.Sum(o => o.PrecoUnitario * o.Quantidade);
            return Math.Round(totalValor / totalQuantidade, 2);
        }
    }
}