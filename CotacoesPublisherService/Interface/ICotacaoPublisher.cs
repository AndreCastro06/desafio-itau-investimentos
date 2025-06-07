using CotacoesPublisherService.DTOs;

namespace CotacoesPublisherService.Interfaces
{
    public interface ICotacaoPublisher
    {
        Task PublicarAsync(CotacaoKafkaDTO cotacao, CancellationToken cancellationToken);
    }
}