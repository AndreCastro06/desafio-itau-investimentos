using CotacoesKafkaWorker.DTOs;

namespace CotacoesKafkaWorker.Interfaces
{ 
    public interface ICotacaoService
    {
        Task<bool> CotacaoExisteAsync(string ativo, DateTime dataHora);
        Task SalvarCotacaoAsync(CotacaoKafkaDTO cotacao);
    }
}