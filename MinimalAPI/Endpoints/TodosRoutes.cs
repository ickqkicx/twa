using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulApi.Data;
using RestfulApi.Data.Entities;

namespace RestfulApi.Endpoints;

static class TodosRoutes
{
    static public async Task<IResult> All(TodoContext context, int page = 1, int size = 10, TodoItemFilter? filter = null)
    {
        var ftodos = context.Todos.AsQueryable();
        if (filter?.Title?.Length >= 2) ftodos = ftodos.Where(x => x.Title.Contains(filter.Title));
        if (filter?.IsComplete != null) ftodos = ftodos.Where(x => x.IsComplete == filter.IsComplete);

        var count = await ftodos.CountAsync();

        var todos = await ftodos.Skip((page - 1) * size).Take(size).ToListAsync();
        return TypedResults.Ok(new { count, todos });
    }

    static public async Task<IResult> ById(TodoContext context, int id)
    {
        var todoItem = await context.Todos.FindAsync(id);

        if (todoItem == null) return TypedResults.NotFound();

        return TypedResults.Ok(todoItem);
    }

    static public async Task<IResult> Post(TodoContext context, [FromBody] TodoItemCreate create)
    {
        var todoItem = create.TodoItem();
        context.Todos.Add(todoItem);
        await context.SaveChangesAsync();

        return TypedResults.Created(nameof(Post), new { id = todoItem.Id });
    }

    static public async Task<IResult> Patch(TodoContext context, int id, [FromBody] TodoItemUpdate update)
    {
        var todoItem = await context.Todos.FindAsync(id);
        if (todoItem == null) return TypedResults.NotFound();
        todoItem += update;

        context.Entry(todoItem).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (context.Todos.Any(e => e.Id == id) == false) return TypedResults.NotFound();
            throw;
        }

        return TypedResults.NoContent();
    }

    static public async Task<IResult> Delete(TodoContext context, int id)
    {
        var todoItem = await context.Todos.FindAsync(id);
        if (todoItem == null) return TypedResults.NotFound();

        context.Todos.Remove(todoItem);
        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static public WebApplication MapTodosRoutes(this WebApplication app)
    {
        var todos = app.MapGroup("todos");
        todos.MapGet(string.Empty, All);
        todos.MapGet("{id}", ById);
        todos.MapPost(string.Empty, Post);
        todos.MapPatch("{id}", Patch);
        todos.MapDelete("{id}", Delete);

        return app;
    }
}