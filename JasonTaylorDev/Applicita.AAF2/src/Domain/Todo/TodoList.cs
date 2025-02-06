using Applicita.AAF2.Domain.Shared;
using Ardalis.SharedKernel;

namespace Applicita.AAF2.Domain.Todo;

public class TodoList : BaseAuditableEntity, IAggregateRoot
{
    public string? Title { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}
