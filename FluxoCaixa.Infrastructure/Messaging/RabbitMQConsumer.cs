using FluxoCaixa.Application.Interfaces;
using FluxoCaixa.Domain.Events;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoCaixa.Infrastructure.Messaging
{
    /*
   public class RabbitMQConsumer : BackgroundService
   {

       private readonly IConnection _connection;
       private readonly IModel _channel;
       private readonly IServiceProvider _serviceProvider;
       private const string _exchangeName = "fluxocaixa_events";
       private const string _queueName = "fluxocaixa_consolidado_queue";

       public RabbitMQConsumer(string connectionString, IServiceProvider serviceProvider)
       {
           var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
           _connection = factory.CreateConnection();
           _channel = _connection.CreateModel();
           _serviceProvider = serviceProvider;

           _channel.ExchangeDeclare(
               exchange: _exchangeName,
               type: ExchangeType.Topic,
               durable: true,
               autoDelete: false);

           _channel.QueueDeclare(
               queue: _queueName,
               durable: true,
               exclusive: false,
               autoDelete: false);

           _channel.QueueBind(
               queue: _queueName,
               exchange: _exchangeName,
               routingKey: nameof(LancamentoCriadoEvent));
       }

       protected override Task ExecuteAsync(CancellationToken stoppingToken)
       {
           var consumer = new EventingBasicConsumer(_channel);

           consumer.Received += async (model, ea) =>
           {
               var body = ea.Body.ToArray();
               var message = Encoding.UTF8.GetString(body);
               var routingKey = ea.RoutingKey;

               try
               {
                   await ProcessarMensagem(routingKey, message);
                   _channel.BasicAck(ea.DeliveryTag, false);
               }
               catch (Exception)
               {
                   _channel.BasicNack(ea.DeliveryTag, false, true);
               }
           };

           _channel.BasicConsume(
               queue: _queueName,
               autoAck: false,
               consumer: consumer);

           return Task.CompletedTask;
       }

       private async Task ProcessarMensagem(string routingKey, string message)
       {
           using var scope = _serviceProvider.CreateScope();

           if (routingKey == nameof(LancamentoCriadoEvent))
           {
               var evento = JsonSerializer.Deserialize<LancamentoCriadoEvent>(message);
               var consolidadoService = scope.ServiceProvider.GetRequiredService<IConsolidadoService>();

               await consolidadoService.ProcessarLancamentoAsync(
                   evento.LancamentoId,
                   evento.Data,
                   evento.Valor,
                   evento.Tipo.ToString());
           }
       }

       public override void Dispose()
       {
           _channel?.Dispose();
           _connection?.Dispose();
           base.Dispose();
       }
       
}
     */
}