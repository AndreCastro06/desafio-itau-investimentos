using CotacoesPublisherService.Interfaces;
using CotacoesPublisherService.Services;
using CotacoesPublisherService.Workers;
using InvestmentControlApi.Infrastructure; 
using Microsoft.EntityFrameworkCore;
using InvestmentControlApi.Infrastructure.Data;
using InvestmentControlApi.Application.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddHttpClient("B3", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddDbContext<InvestmentDbContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")))
        );

        services.AddSingleton<ICotacaoPublisher, CotacaoPublisher>();
      

        services.AddHostedService<CotacaoScheduler>();
    })
    .Build();

host.Run();