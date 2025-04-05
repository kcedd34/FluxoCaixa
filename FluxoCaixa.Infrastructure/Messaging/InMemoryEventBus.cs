using FluxoCaixa.Application.Interfaces;
using FluxoCaixa.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class InMemoryEventBus : IEventBus
{
    private readonly ILogger<InMemoryEventBus> _logger;
    private readonly IServiceProvider _serviceProvider;

    public InMemoryEventBus(ILogger<InMemoryEventBus> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task PublicarAsync<T>(T evento) where T : DomainEvent
    {
        _logger.LogInformation("Evento publicado: {EventType}", typeof(T).Name);

        // Processamento síncrono para simulação
        if (evento is LancamentoCriadoEvent lancamentoEvento)
        {
            using var scope = _serviceProvider.CreateScope();
            var consolidadoService = scope.ServiceProvider.GetRequiredService<IConsolidadoService>();

            return consolidadoService.ProcessarLancamentoAsync(
                lancamentoEvento.LancamentoId,
                lancamentoEvento.Data,
                lancamentoEvento.Valor,
                lancamentoEvento.Tipo.ToString());
        }

        return Task.CompletedTask;
    }
}