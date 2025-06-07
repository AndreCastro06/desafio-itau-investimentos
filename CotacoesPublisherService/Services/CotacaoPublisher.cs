using Confluent.Kafka;
using CotacoesPublisherService.DTOs;
using CotacoesPublisherService.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading;

namespace CotacoesPublisherService.Services
{
    public class CotacaoPublisher : ICotacaoPublisher
    {
        private readonly IProducer<string, string> _producer;

        public CotacaoPublisher(IConfiguration config)
        {
            var configKafka = new ProducerConfig
            {
                BootstrapServers = config["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<string, string>(configKafka).Build();
        }

        public async Task PublicarAsync(CotacaoKafkaDTO cotacao, CancellationToken cancellationToken)
        {
            var mensagem = JsonSerializer.Serialize(cotacao);
            await _producer.ProduceAsync("cotacoes-novas", new Message<string, string>
            {
                Key = cotacao.Ativo,
                Value = mensagem
            }, cancellationToken);
        }
    }
}