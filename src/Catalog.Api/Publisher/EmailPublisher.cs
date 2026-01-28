using RabbitMQ.Client;

namespace Catalog.Api.Publisher;

public class EmailPublisher 
{
    private readonly IConfiguration _configuration;
    public EmailPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task Publish(EmailNotificationEvent message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["MessageBroker:HostName"],
            UserName = _configuration["MessageBroker:UserName"],
            Password = _configuration["MessageBroker:Password"],
            VirtualHost = _configuration["MessageBroker:VirtualHost"],

            Port = int.Parse(_configuration["MessageBroker:Port"]!),
            AutomaticRecoveryEnabled = true,

            Ssl = new SslOption
            {
                Enabled = bool.Parse(_configuration["MessageBroker:UseSsl"]!),
                ServerName = _configuration["MessageBroker:HostName"],


                AcceptablePolicyErrors =
                        System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors |
                        System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch |
                        System.Net.Security.SslPolicyErrors.RemoteCertificateNotAvailable
            }
        };

        using var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: _configuration["MessageBroker:EmailQueue"]!,
                               durable: true,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

        var json = System.Text.Json.JsonSerializer.Serialize(message);
        var body = System.Text.Encoding.UTF8.GetBytes(json);
        var props = new BasicProperties();

        await channel.BasicPublishAsync(
                exchange: "",
                routingKey: _configuration["MessageBroker:EmailQueue"]!, mandatory: true,
                basicProperties: props, body: body
                 );

    }

     

}
