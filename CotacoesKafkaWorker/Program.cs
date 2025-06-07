using Microsoft.EntityFrameworkCore;
using InvestmentControlApi.Infrastructure.Data;
using CotacoesKafkaWorker.Services;
using CotacoesKafkaWorker.Interfaces;
using CotacoesKafkaWorker.Consumers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<InvestmentDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddScoped<ICotacaoService, CotacaoService>();
        services.AddHostedService<CotacaoKafkaConsumer>();
    })
    .Build();

await host.RunAsync();