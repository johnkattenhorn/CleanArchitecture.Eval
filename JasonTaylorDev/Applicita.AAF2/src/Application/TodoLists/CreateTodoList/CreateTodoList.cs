using Applicita.AAF2.Application.Common.Interfaces;
using Applicita.AAF2.Domain.Todo;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Applicita.AAF2.Application.TodoLists.CreateTodoList;

public record CreateTodoListCommand : ICommand<Result<int>>
{
    public string? Title { get; init; }
}

public class CreateTodoListCommandHandler(IRepository<TodoList> repository) : ICommandHandler<CreateTodoListCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList
        {
            Title = request.Title
        };

        var createdToList = await repository.AddAsync(entity, cancellationToken);

        return createdToList.Id;
    }
}
