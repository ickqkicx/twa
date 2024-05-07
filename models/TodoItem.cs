using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Models;

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

// [DataType(DataType.Text) Display(Name = "Bob") RegularExpression("rs")]

public class TodoItemCreate
{
    [Required, StringLength(10, MinimumLength = 3)]
    public string Title { get; init; }

    public TodoItem TodoItem() => new() { Title = this.Title };
}
public class TodoItemUpdate
{
    [StringLength(10, MinimumLength = 3)]
    public string? Title { get; init; }
    public bool? IsComplete { get; init; }
}
public class TodoItemFilter
{
    [StringLength(10, MinimumLength = 2)]
    public string? Title { get; init; }
    public bool? IsComplete { get; init; }
}
