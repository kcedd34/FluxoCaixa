using System;

namespace FluxoCaixa.Domain.Events
{
    public abstract class DomainEvent
    {
        public Guid Id { get; }
        public DateTime OcorridoEm { get; }

        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            OcorridoEm = DateTime.UtcNow;
        }
    }
}