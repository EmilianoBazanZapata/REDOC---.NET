using ApiRedoc.ViewModels.Request;
using Microsoft.AspNetCore.Mvc;

namespace ApiRedoc.Controllers;

[ApiExplorerSettings(GroupName = "swagger")]
[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private static readonly List<TodoItemRequest> todos = new List<TodoItemRequest>();
    private static int nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<TodoItemRequest>> Get()
    {
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public ActionResult<TodoItemRequest> Get(int id)
    {
        var todo = todos.Find(t => t.Id == id);

        if (todo == null)
            return NotFound();

        return Ok(todo);
    }

    [HttpPost]
    public ActionResult Post([FromBody] TodoItemRequest todo)
    {
        if (todos.Exists(t => t.Name == todo.Name))
            return Conflict($"Already exists the Todo: { todo.Name }");

        // A counter instead of todos.Count + 1, so ids are never reused after a delete.
        todo.Id = nextId++;
        todos.Add(todo);

        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] TodoItemRequest updatedTodo)
    {
        var todo = todos.Find(t => t.Id == id);

        if (todo == null)
            return NotFound();

        // Check the name against the other tasks before changing anything.
        if (todos.Exists(t => t.Id != id && t.Name == updatedTodo.Name))
            return Conflict($"Already exists the Todo: { updatedTodo.Name }");

        todo.Name = updatedTodo.Name;
        todo.Completed = updatedTodo.Completed;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var todo = todos.Find(t => t.Id == id);

        if (todo == null)
            return NotFound();

        todos.Remove(todo);

        return NoContent();
    }
}
