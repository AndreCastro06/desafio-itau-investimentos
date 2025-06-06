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
                throw new ArgumentException("Lista inválida.");

            var compras = operacoes.Where(o => o.Tipo == TipoOperacao.Compra).ToList();
            if (!compras.Any()) throw new ArgumentException("Sem compras.");

            var totalQtd = compras.Sum(o => o.Quantidade);
            if (totalQtd == 0) throw new InvalidOperationException("Qtd = 0.");

            var totalValor = compras.Sum(o => o.PrecoUnitario * o.Quantidade);
            return Math.Round(totalValor / totalQtd, 2);
        }
    }
}