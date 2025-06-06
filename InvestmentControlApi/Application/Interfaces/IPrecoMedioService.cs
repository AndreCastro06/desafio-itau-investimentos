using InvestmentControlApi.Domain.Entities;

namespace InvestmentControlApi.Application.Interfaces
{
    
    public interface IPrecoMedioService
    {
        decimal CalcularPrecoMedio(List<Operacao> operacoes);
    }
}
