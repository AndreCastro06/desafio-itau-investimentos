using InvestmentControlApi.Infrastructure.Data;
using InvestmentControlApi.Application.Interfaces;
using InvestmentControlApi.Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InvestmentDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

builder.Services.AddScoped<IPosicaoService, PosicaoService>();
builder.Services.AddScoped<IPrecoMedioService, PrecoMedioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("https://localhost:5183/swagger/v1/swagger.json", "InvestmentControlApi v1");
        c.RoutePrefix = string.Empty; 
    });
}
Console.WriteLine(" Connection string usada:");
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
