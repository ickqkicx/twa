using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestfulAPI.Data.Entities;

[Table("TodoItem", Schema = "dbo")]
public class TodoItem
{
    public int Id { get; init; }

    public string Title { get; set; }

    public bool IsComplete { get; set; } = false;

    public static TodoItem operator +(TodoItem cur, TodoItemUpdate update)
    {
        if (update.Title != null) cur.Title = update.Title;
        if (update.IsComplete.HasValue) cur.IsComplete = (bool)update.IsComplete;
        return cur;
    }
}

public class TodoItemCreate
{
    [Required, DataType(DataType.Text), StringLength(10, MinimumLength=3)]
    public string Title { get; init; }

    public TodoItem TodoItem() => new() { Title = this.Title };
}
public class TodoItemUpdate
{
    [MinLength(3), MaxLength(10)]
    public string? Title { get; init; }
    public bool? IsComplete { get; init; }
}
public class TodoItemFilter
{
    [MinLength(2), MaxLength(10)]
    public string? Title { get; init; }
    public bool? IsComplete { get; init; }
}
