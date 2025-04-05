using FluxoCaixa.Infrastructure.Resilience;
using System.Net;
using System.Text.Json;

namespace FluxoCaixa.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);

            var code = HttpStatusCode.InternalServerError; // 500 por padrão
            var message = "Ocorreu um erro interno no servidor.";

            if (exception is ArgumentException)
            {
                code = HttpStatusCode.BadRequest; // 400
                message = exception.Message;
            }
            else if (exception is CircuitBreakerOpenException)
            {
                code = HttpStatusCode.ServiceUnavailable; // 503
                message = "Serviço temporariamente indisponível. Tente novamente mais tarde.";
            }

            var result = JsonSerializer.Serialize(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}