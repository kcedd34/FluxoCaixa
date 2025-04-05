using FluxoCaixa.Application.Interfaces;
using FluxoCaixa.Domain.Events;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FluxoCaixa.Infrastructure.Messaging
{
    /*
   public class RabbitMQEventBus : IEventBus, IDisposable
   {

       private readonly IConnection _connection;
       private readonly IModel _channel;
       private const string _exchangeName = "fluxocaixa_events";

       public RabbitMQEventBus(string connectionString)
       {
           var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
           _connection = factory.CreateConnection();
           _channel = _connection.CreateModel();

           _channel.ExchangeDeclare(
               exchange: _exchangeName,
               type: ExchangeType.Topic,
               durable: true,
               autoDelete: false);
       }

       public Task PublicarAsync<T>(T evento) where T : DomainEvent
       {
           var eventName = evento.GetType().Name;
           var message = JsonSerializer.Serialize(evento);
           var body = Encoding.UTF8.GetBytes(message);

           _channel.BasicPublish(
               exchange: _exchangeName,
               routingKey: eventName,
               basicProperties: null,
               body: body);

           return Task.CompletedTask;
       }

       public void Dispose()
       {
           _channel?.Dispose();
           _connection?.Dispose();
       }
       
}
    */
}