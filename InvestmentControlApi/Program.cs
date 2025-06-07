using InvestmentControlApi.Infrastructure.Data;
using InvestmentControlApi.Application.Interfaces;
using InvestmentControlApi.Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco em memória por enquanto
builder.Services.AddDbContext<InvestmentDbContext>(options =>
    options.UseInMemoryDatabase("InvestmentDb"));

// Serviços de aplicação
builder.Services.AddScoped<IPosicaoService, PosicaoService>();
builder.Services.AddScoped<IPrecoMedioService, PrecoMedioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "InvestmentControlApi v1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
