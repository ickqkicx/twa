using GrpcTodoCore.Proto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<Todos.TodosClient>(option =>
{
    option.Address = new Uri("localhost");
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
