using FluxoCaixa.API.Middleware;
using FluxoCaixa.Application.Interfaces;
using FluxoCaixa.Application.Services;
using FluxoCaixa.Domain.Interfaces;
using FluxoCaixa.Infrastructure.Cache;
using FluxoCaixa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Adiciona Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework
builder.Services.AddDbContext<FluxoCaixaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "FluxoCaixa:";
});

// Inje��o de Depend�ncia - Reposit�rios
builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();
builder.Services.AddScoped<IConsolidadoRepository, ConsolidadoRepository>();

// Inje��o de Depend�ncia - Servi�os
builder.Services.AddScoped<ILancamentoService, LancamentoService>();
builder.Services.AddScoped<IConsolidadoService, ConsolidadoService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// Servi�os de Mensageria
//builder.Services.AddSingleton<IEventBus>(provider =>
//    new RabbitMQEventBus(builder.Configuration.GetConnectionString("RabbitMQConnection")));

builder.Services.AddScoped<IEventBus, InMemoryEventBus>();

var app = builder.Build();

// Middleware para gerenciar exce��es globais
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Cria o banco de dados e aplica migra��es
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FluxoCaixaDbContext>();
    dbContext.Database.Migrate();
}

app.Run();