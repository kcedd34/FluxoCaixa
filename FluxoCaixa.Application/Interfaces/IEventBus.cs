using FluxoCaixa.Domain.Events;
using System.Threading.Tasks;

namespace FluxoCaixa.Application.Interfaces
{
    public interface IEventBus
    {
        Task PublicarAsync<T>(T evento) where T : DomainEvent;
    }
}