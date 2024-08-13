using GrpcTodoCore.Proto;
using Microsoft.AspNetCore.Mvc;

namespace GrpcTodoClientAspNet.Controllers;

[ApiController]
[Route("api/todos")]
public class ContentGrpcController(Todos.TodosClient grpcClient) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTodoItems(int page = 1, int size = 10, [FromQuery] GetTodoItemsRequest.Types.TodoItemFilter filter = null)
    {
        var todos = await grpcClient.GetTodoItemsAsync(new GetTodoItemsRequest { Page = page, Size = size, Filter = filter });

        return Ok(new {todos.Count, todos.TodoItems});
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
    {
        try
        {
            var todoItem = await grpcClient.GetTodoItemAsync(new TodoItemRequest { Id = id });

            return Ok(todoItem);
        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostTodoItem([FromBody] PostTodoItemRequest create)
    {
        var todoItem = await grpcClient.PostTodoItemAsync(create);

        return Ok(todoItem);
    }

    public record struct TodoItemUpdate(string title, bool isComplete);

    [HttpPut("{id}")]
    public async Task<ActionResult> PutTodoItem(int id, [FromBody] TodoItemUpdate update)
    {
        try
        {
            var todoItem = await grpcClient.PutTodoItemAsync(new TodoItem { Id = id, Title = update.title, IsComplete = update.isComplete });

            return Ok(todoItem);
        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodoItem(int id)
    {
        try
        {
            var todoItem = await grpcClient.DeleteTodoItemAsync(new TodoItemRequest { Id = id });

            return Ok(todoItem);
        }
        catch(Exception ex)
        {
            return NotFound();
        }
    }
}
