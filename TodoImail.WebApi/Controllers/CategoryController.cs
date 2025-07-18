using Microsoft.AspNetCore.Mvc;
using TodoImail.Services.Models;
using TodoImail.Services.Services;
using TodoImail.WebApi.Contracts.Dtos;
using TodoImail.WebApi.Filters;
using TodoImail.WebApi.Helpers;

namespace TodoImail.WebApi.Controllers; 
[Route("category")]
public class CategoryController : ControllerBase {
    private readonly ITodoImailService _service;
    public ControllerHelper Helper { get; init; } = new ();

    public CategoryController(ITodoImailService service) {
        _service = service;
    }

    [HttpGet, Route("")]
    public async Task<IEnumerable<CategoryDto>> GetAll() {
        return (await _service.ListCategories()).Select(Helper.ToCategoryDto);
    }

    [HttpGet, Route("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetOne([FromRoute(Name = "id")] int categoryId) {
        try {
            return Helper.ToCategoryDto(await _service.GetCategory(categoryId));
        } catch (TodoImailServiceException ex) when (ex.Code == 1) {
            return NotFound(ex.Message);
        }
    }

    [HttpGet, Route("{label}")]
    public async Task<ActionResult<CategoryDto>> GetOne([FromRoute(Name = "label")] string label) {
        try {
            return Helper.ToCategoryDto(await _service.GetCategory(label));
        } catch (TodoImailServiceException ex) when (ex.Code == 1) {
            return NotFound(ex.Message);
        }
    }

    [HttpGet, Route("{id:int}/todo")]
    [TodoImailExceptionFilter]
    public async Task<IEnumerable<TodoDto>> GetTodos([FromRoute(Name = "id")] int categoryId) {
        return (await _service.ListTodos(categoryId)).Select(Helper.ToTodoDto);
    }
}
