using Microsoft.AspNetCore.Mvc;
using TodoImail.Services.Services;
using TodoImail.WebApi.Contracts.Dtos;
using TodoImail.WebApi.Filters;
using TodoImail.WebApi.Helpers;

namespace TodoImail.WebApi.Controllers; 
[Route("[controller]")]
[ApiController] // FromQuery pour les types de base, FromBody pour les object, Validation -> 400
[TodoImailExceptionFilter]
public class TodoController : ControllerBase {
    private readonly ITodoImailService _service;
    public ControllerHelper Helper { get; init; } = new();

    public TodoController(ITodoImailService service) {
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<TodoDto>> GetMany(string? filter) {
        var req = string.IsNullOrWhiteSpace(filter) ? _service.ListTodos() : _service.SearchTodos(filter);
        return (await req).Select(Helper.ToTodoDto);
    }

    [HttpGet, Route("{todoId:int:min(1)}", Name = "GetTodoById")]
    public async Task<TodoDetailDto> GetOne(int todoId) {
        return Helper.ToTodoDetailDto(await _service.GetTodo(todoId));
    }

    [HttpDelete, Route("{todoId:int:min(1)}")]
    public async Task<ActionResult> DeleteOne(int todoId) {
        await _service.RemoveTodo(todoId);
        return NoContent();
    }

    [HttpPost, Route("")]
    public async Task<ActionResult> PostOne(TodoPostDto dto) {
        var model = await _service.CreateTodo(Helper.ToTodoCreate(dto));
        return CreatedAtRoute("GetTodoById", new { todoId = model.Id }, Helper.ToTodoDetailDto(model));
        //return CreatedAtAction(nameof(GetOne), "Todo", new { todoId = model.Id }, Helper.ToTodoDetailDto(model));
    }

    [HttpPut, Route("{todoId:int:min(1)}")]
    public async Task<TodoDetailDto> PustOne(int todoId, TodoPutDto dto) {
        var model = await _service.ModifyTodo(Helper.ToTodoModify(todoId, dto));
        return Helper.ToTodoDetailDto(model);
    }

    [HttpPatch, Route("{todoId:int:min(1)}")]
    public async Task<TodoDetailDto> PatchOne(int todoId, TodoPatchDto dto) {
        var model = await _service.GetTodo(todoId);
        if(model.IsDone != dto.IsDone!.Value)
            model = await _service.ToggleTodo(todoId);
        return Helper.ToTodoDetailDto(model);
    }
}
