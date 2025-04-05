using System;
using System.Threading.Tasks;

namespace FluxoCaixa.Infrastructure.Resilience
{
    public class CircuitBreaker<T>
    {
        private readonly Func<Task<T>> _operation;
        private readonly int _maxFailures;
        private readonly TimeSpan _resetTimeout;

        private int _failureCount;
        private DateTime _lastFailureTime;
        private CircuitState _state;

        public enum CircuitState
        {
            Closed,
            Open,
            HalfOpen
        }

        public CircuitBreaker(Func<Task<T>> operation, int maxFailures, TimeSpan resetTimeout)
        {
            _operation = operation;
            _maxFailures = maxFailures;
            _resetTimeout = resetTimeout;
            _state = CircuitState.Closed;
        }

        public async Task<T> ExecuteAsync()
        {
            if (_state == CircuitState.Open)
            {
                // Verifica se já passou o tempo de reset
                if (DateTime.UtcNow - _lastFailureTime > _resetTimeout)
                {
                    _state = CircuitState.HalfOpen;
                }
                else
                {
                    throw new CircuitBreakerOpenException("Circuit breaker está aberto");
                }
            }

            try
            {
                var result = await _operation();

                // Se estiver meio-aberto e operação sucedeu, fecha o circuito
                if (_state == CircuitState.HalfOpen)
                {
                    _state = CircuitState.Closed;
                    _failureCount = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                _lastFailureTime = DateTime.UtcNow;
                _failureCount++;

                if (_state == CircuitState.HalfOpen || _failureCount >= _maxFailures)
                {
                    _state = CircuitState.Open;
                }

                throw;
            }
        }
    }

    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException(string message) : base(message) { }
    }
}