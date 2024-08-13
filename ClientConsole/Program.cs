using Grpc.Core;
using Grpc.Net.Client;
using GrpcTodoCore.Proto;

Console.WriteLine("Hello, World!");


using var channel = GrpcChannel.ForAddress("localhost");
var client = new Todos.TodosClient(channel);

var q = await client.GetTodoItemsAsync(new GetTodoItemsRequest() { Page = 1, Size = 4 });
var w = await client.GetTodoItemAsync(new TodoItemRequest() { Id = 1 });
var e = await client.PostTodoItemAsync(new PostTodoItemRequest() { Title = "Hello new element" });
var r = await client.PutTodoItemAsync(new TodoItem() { Id = e.Id, Title = "Update element", IsComplete = true });
var t = await client.DeleteTodoItemAsync(new TodoItemRequest() { Id = r.Id });

var a = client.GetTodoItemsStream(new GetTodoItemsRequest() { Page = 1, Size = 4 });
var s = a.ResponseStream;
while (await s.MoveNext(CancellationToken.None))
    Console.WriteLine($"{s.Current.Id} - {s.Current.Title} - {s.Current.IsComplete}");


var d = client.Duplex();
var responseReaderTask = Task.Run(async () =>
{
    //while (await d.ResponseStream.MoveNext(CancellationToken.None))
    //{
    //    var num = d.ResponseStream.Current;
    //    await Console.Out.WriteLineAsync(num.X.ToString());
    //    if (num.X >= 6)
    //    {
    //        await d.RequestStream.CompleteAsync();
    //        break;
    //    }
    //    num.X -= 1;
    //    await d.RequestStream.WriteAsync(num);
    //}

    await foreach (var num in d.ResponseStream.ReadAllAsync())
    {
        await Console.Out.WriteLineAsync(num.X.ToString());
        if (num.X >= 6)
        {
            await d.RequestStream.CompleteAsync();
            break;
        }
        num.X -= 1;
        await d.RequestStream.WriteAsync(num);
    }
});

await d.RequestStream.WriteAsync(new Num { X = 2 });
await responseReaderTask;


Console.WriteLine("Hello, World!");