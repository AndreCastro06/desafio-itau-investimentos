using CotacoesPublisherService.DTOs;
using CotacoesPublisherService.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CotacoesPublisherService.Workers
{
    public class CotacaoScheduler : BackgroundService
    {
        private readonly ILogger<CotacaoScheduler> _logger;
        private readonly ICotacaoPublisher _publisher;
        private readonly IHttpClientFactory _httpFactory;

        public CotacaoScheduler(
            ILogger<CotacaoScheduler> logger,
            ICotacaoPublisher publisher,
            IHttpClientFactory httpFactory)
        {
            _logger = logger;
            _publisher = publisher;
            _httpFactory = httpFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var ativos = new[] { "PETR4", "VALE3", "ITUB4", "BBDC4", "MGLU3" };

                    foreach (var ativo in ativos)
                    {
                        var client = _httpFactory.CreateClient("B3");
                        var response = await client.GetStringAsync($"https://b3api.vercel.app/api/Assets/{ativo.ToUpper()}");

                        var json = JsonDocument.Parse(response).RootElement;

                        var dto = new CotacaoKafkaDTO
                        {
                            Ativo = json.GetProperty("ticker").GetString()!,
                            Preco = json.GetProperty("price").GetDecimal(),
                            DataHora = json.GetProperty("tradetime").GetDateTime()
                        };

                        await _publisher.PublicarAsync(dto, stoppingToken);
                        _logger.LogInformation("Publicado ativo {0} com preço R$ {1} às {2}", dto.Ativo, dto.Preco, dto.DataHora);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao consultar API ou publicar no Kafka");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}