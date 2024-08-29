using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "192.168.1.67" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("topic_logs", ExchangeType.Topic, false, true);

var queueName = channel.QueueDeclare().QueueName;
if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: {0} [binding_key...]",
                            Environment.GetCommandLineArgs()[0]);
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
    Environment.ExitCode = 1;
    return;
}

foreach (var bindingKey in args)
    channel.QueueBind(queueName, "topic_logs", bindingKey);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
};

channel.BasicConsume(queueName, true, consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

// dotnet run "#" - все журналы
// dotnet run "kern.*" - журналы для kern
// dotnet run "*.critical" - только critical журналы
// привязки можно комбинировать