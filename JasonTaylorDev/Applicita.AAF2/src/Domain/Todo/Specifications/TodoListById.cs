using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Applicita.AAF2.Domain.Todo.Specifications;
public class TodoListByIdSpec : Specification<TodoList>
{
    public TodoListByIdSpec(int todoListById) =>
        Query
            .Where(todolist => todolist.Id == todoListById);
}
