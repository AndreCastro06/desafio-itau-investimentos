using InvestmentControlApi.Infrastructure.Data;
using InvestmentControlApi.Application.Interfaces;
using InvestmentControlApi.Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco em mem�ria por enquanto
builder.Services.AddDbContext<InvestmentDbContext>(options =>
    options.UseInMemoryDatabase("InvestmentDb"));

// Servi�os de aplica��o
builder.Services.AddScoped<IPrecoMedioService, PrecoMedioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();