using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulApi.Data;
using RestfulApi.Data.Entities;

namespace RestfulApi.Controllers;

// if (!ModelState.IsValid) return BadRequest(ModelState);
// ModelState.AddModelError()

[ApiController]
[Route("api/todos")]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<object>> GetTodoItems(int page = 1, int size = 10, [FromQuery] TodoItemFilter? filter = null)
    {
        var ftodos = _context.Todos.AsQueryable();
        if (filter?.Title?.Length >= 2) ftodos = ftodos.Where(x => x.Title.Contains(filter.Title));
        if (filter?.IsComplete != null) ftodos = ftodos.Where(x => x.IsComplete == filter.IsComplete);

        var count = await ftodos.CountAsync();

        var todos = await ftodos.Skip((page - 1) * size).Take(size).ToListAsync();
        return new { count, todos };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
    {
        var todoItem = await _context.Todos.FindAsync(id);
        if (todoItem == null) return NotFound();

        return todoItem;
    }

    [HttpPost]
    public async Task<ActionResult<object>> PostTodoItem([FromBody] TodoItemCreate create)
    {
        var todoItem = create.TodoItem();
        _context.Todos.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(PostTodoItem), new { id = todoItem.Id });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTodoItem(int id, [FromBody] TodoItemUpdate update)
    {
        var todoItem = await _context.Todos.FindAsync(id);
        if (todoItem == null) return NotFound();
        todoItem += update;

        _context.Entry(todoItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_context.Todos.Any(e => e.Id == id) == false) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var todoItem = await _context.Todos.FindAsync(id);
        if (todoItem == null) return NotFound();

        _context.Todos.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}