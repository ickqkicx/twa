using GrpcTodoServer.Data;
using GrpcTodoServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<TodoContext>();

var app = builder.Build();

app.MapGrpcService<GreeterService>();

app.Run();