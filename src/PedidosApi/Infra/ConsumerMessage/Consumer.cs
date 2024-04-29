using Newtonsoft.Json;
using PedidosApi.Domain.Entities;
using PedidosApi.Domain.Enums;
using PedidosApi.Domain.Models;
using PedidosApi.Domain.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PedidosApi.Infra.ConsumerMessage
{
    internal class Consumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger _logger;

        public Consumer(IServiceScopeFactory serviceScopeFactory, ILogger<Consumer> logger, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var factory = new ConnectionFactory { HostName = configuration.GetValue<string>("Rabbit:Host"), Port = configuration.GetValue<int>("Rabbit:Port"), DispatchConsumersAsync = true };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            InitQueues();

            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RecivePedido();

            return Task.CompletedTask;
        }

        private void RecivePedido()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    byte[] body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    using var scope = _serviceScopeFactory.CreateScope();
                    var pedidoService = scope.ServiceProvider.GetRequiredService<IPedidoService>();

                    var pedido = JsonConvert.DeserializeObject<Pedido>(message) ?? new();
                    (_, PedidoResponse? pedidoAdded) = await pedidoService.AddAsync(pedido);

                    if (pedidoAdded is null || pedidoAdded.Status != StatusPedido.EmAnalise)
                    {
                        if (pedidoService.ErrorsMessage.Count == 0)
                        {
                            _channel.BasicNack(ea.DeliveryTag, false, false);
                            return;
                        }

                        pedido.Situacao = string.Join(", ", pedidoService.ErrorsMessage.ToArray());
                        var response = JsonConvert.SerializeObject(pedido);
                        var pedidoResponse = Encoding.UTF8.GetBytes(response);
                        _channel.BasicPublish(exchange: "RejectedExchange", routingKey: string.Empty, body: pedidoResponse);
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(message: ex.Message, exception: ex);
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };
            _channel.BasicConsume(queue: "Pedidos",
                                 autoAck: false,
                                 consumer: consumer);
        }

        private void InitQueues()
        {
            _channel.ExchangeDeclare(exchange: "RejectedExchange", type: ExchangeType.Direct);
            _channel.QueueDeclare("RejectedQueue", true, false, false);
            _channel.QueueBind(queue: "RejectedQueue", exchange: "RejectedExchange", routingKey: string.Empty);

            _channel.ExchangeDeclare(exchange: "DeadLetterExchance", type: ExchangeType.Direct);
            _channel.QueueDeclare("DeadLetterQueue", true, false, false);
            _channel.QueueBind(queue: "DeadLetterQueue", exchange: "DeadLetterExchance", routingKey: string.Empty);

            var options = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "DeadLetterExchance" }
            };

            _channel.ExchangeDeclare(exchange: "AcertoExchance", type: ExchangeType.Direct, arguments: options);
            _channel.QueueDeclare("Pedidos", false, false, false, options);
            _channel.QueueBind(queue: "Pedidos", exchange: "AcertoExchance", routingKey: string.Empty);
        }
    }
}
