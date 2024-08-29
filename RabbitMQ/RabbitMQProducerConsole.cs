using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("topic_logs", ExchangeType.Topic, false, true);

var routingKey = (args.Length > 0) ? args[0] : "anon.info";
var message = (args.Length > 1) ? string.Join(" ", args.Skip(1).ToArray()) : "Hello World!";

var body = Encoding.UTF8.GetBytes(message);
channel.BasicPublish("topic_logs", routingKey, null, body);

Console.WriteLine($" [x] Sent '{routingKey}':'{message}'");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

// dotnet run "kern.warn" "1"
// dotnet run "kern.critical" "1"
// dotnet run "foo.warn" "1"
// dotnet run "foo.critical" "1"