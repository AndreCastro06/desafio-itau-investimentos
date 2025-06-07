using Confluent.Kafka;
using CotacoesKafkaWorker.DTOs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using CotacoesKafkaWorker.Interfaces;
using InvestmentControlApi.Infrastructure.Data;

namespace CotacoesKafkaWorker.Consumers
{
    public class CotacaoKafkaConsumer : BackgroundService
    {
        private readonly ILogger<CotacaoKafkaConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;
        private readonly IConsumer<string, string> _consumer;

        public CotacaoKafkaConsumer(
            ILogger<CotacaoKafkaConsumer> logger,
            IServiceScopeFactory scopeFactory,
            IConfiguration config)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _config = config;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                GroupId = "cotacoes-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando serviço CotacaoKafkaConsumer.");

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<InvestmentDbContext>();
                var conectado = await context.Database.CanConnectAsync();
                _logger.LogInformation("Conexão com banco estabelecida: {Status}", conectado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar conectar ao banco de dados.");
            }

            _consumer.Subscribe("cotacoes-novas");
            _logger.LogInformation("Inscrito no tópico Kafka: cotacoes-novas");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    var mensagem = result.Message.Value;

                    using var scope = _scopeFactory.CreateScope();
                    var cotacaoService = scope.ServiceProvider.GetRequiredService<ICotacaoService>();

                    var cotacao = JsonSerializer.Deserialize<CotacaoKafkaDTO>(mensagem, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (cotacao == null)
                    {
                        _logger.LogWarning("Mensagem inválida recebida: {Mensagem}", mensagem);
                        continue;
                    }

                    if (await cotacaoService.CotacaoExisteAsync(cotacao.Ativo, cotacao.DataHora))
                    {
                        _logger.LogInformation("Cotação duplicada ignorada: {Ativo} {Data}", cotacao.Ativo, cotacao.DataHora);
                        continue;
                    }

                    await cotacaoService.SalvarCotacaoAsync(cotacao);
                    _logger.LogInformation("Cotação salva com sucesso: {Ativo} R$ {Preco}", cotacao.Ativo, cotacao.Preco);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("Cancelamento do consumo solicitado.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao consumir mensagem do Kafka. Nova tentativa em 5 segundos.");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }
}