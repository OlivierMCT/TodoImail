using System.Net.Http.Json;
using TodoImail.BlazorApp.Client.Models;
using TodoImail.WebApi.Contracts.Dtos;

namespace TodoImail.BlazorApp.Client.Services; 
public interface ITodoImailClientService {
    Task<List<Category>> GetCategories();
    Task<TodoDetail?> GetTodo(int id);
    Task<List<Todo>> GetTodos();
    Task<TodoDetail?> PostTodo(TodoAdd todo);
    Task RemoveTodo(Todo todo);
    Task<TodoDetail?> ToggleTodo(Todo todo);
}

public class TodoImailClientService(HttpClient http) : ITodoImailClientService {
    private readonly HttpClient _http = http;

    private Category ToModel(CategoryDto dto) {
        return new () {
            Id = dto.Id,
            Color = dto.Color,
            Label = dto.Label,
            TodosCount = dto.TodosCount,
            ShortLabel = dto.Label.Length < 15 ? dto.Label : dto.Label[..12] + "...",
        };
    }

    private Todo ToModel(TodoDto dto, Category? category) {
        return new() {
            Id = dto.Id,
            Label = dto.Label,
            IsDone = dto.IsDone,
            Status = dto.Status,
            Category = category
        };
    }

    private TodoDetail ToModel(TodoDetailDto dto) {
        return new() {
            Id = dto.Id,
            Label = dto.Label,
            IsDone = dto.IsDone,
            Status = dto.Status,
            Category = dto.Category is not null ? ToModel(dto.Category) : null,
            DueDate = dto.DueDate,
            IsDeletable = dto.IsDeletable,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            WithCoords = dto.Latitude.HasValue && dto.Longitude.HasValue
        };
    }

    public async Task<List<Category>> GetCategories() {
        if (_cacheCategories.HasValue && (DateTime.Now - _cacheCategories.Value.timestamp < _maxCache))
            return _cacheCategories.Value.data;

        string url = $"{_http.BaseAddress}category";
        List<CategoryDto>? dtos = await _http.GetFromJsonAsync<List<CategoryDto>>(url);
        List<Category>? models = dtos?.Select(ToModel).ToList();
        if (models != null) _cacheCategories = (models, DateTime.Now);
        return models ?? [];
    }

    public async Task<List<Todo>> GetTodos() {
        if (_cacheTodos.HasValue && (DateTime.Now - _cacheTodos.Value.timestamp < _maxCache))
            return _cacheTodos.Value.data; 

        string url = $"{_http.BaseAddress}todo";
        var todosTask = _http.GetFromJsonAsync<List<TodoDto>>(url);
        var categoriesTask = GetCategories();
        await Task.WhenAll(todosTask, categoriesTask);

        var todosDto = todosTask.Result;
        var categoriesModel = categoriesTask.Result;

        var models = todosDto?
            .Select(todo => ToModel(todo, categoriesModel.FirstOrDefault(c => c.Id == todo.CategoryId)))
            .ToList();
        if (models != null) _cacheTodos = (models, DateTime.Now);
        return models ?? [];
    }

    public async Task<TodoDetail?> GetTodo(int id) {
        string url = $"{_http.BaseAddress}todo/{id}";
        var todoDto = await _http.GetFromJsonAsync<TodoDetailDto>(url);
        if (todoDto is null) return null;
        return ToModel(todoDto);
    }

    public async Task<TodoDetail?> ToggleTodo(Todo todo) {
        string url = $"{_http.BaseAddress}todo/{todo.Id}";
        var response = await _http.PatchAsJsonAsync(url, new TodoPatchDto() { IsDone = !todo.IsDone });
        var dto = await response.Content.ReadFromJsonAsync<TodoDetailDto>();
        if (dto is null) return null;
        return ToModel(dto);
    }

    public async Task RemoveTodo(Todo todo) {
        string url = $"{_http.BaseAddress}todo/{todo.Id}";
        var response = await _http.DeleteAsync(url);       
        if(!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.NotFound) {
            throw new Exception(response.StatusCode switch { 
                System.Net.HttpStatusCode.BadRequest => "la tâche n'est pas supprimable",
                System.Net.HttpStatusCode.Unauthorized => "il faut se connecter pour supprimer",
                System.Net.HttpStatusCode.Forbidden => "pas le droit de supprimer",
                _ => "merci de réessayer un peu plus tard"
            });
        }
    }

    public async Task<TodoDetail?> PostTodo(TodoAdd todo) {
        string url = $"{_http.BaseAddress}todo";
        var response = await _http.PostAsJsonAsync(url, new TodoPostDto() {
            CategoryId = todo.CategoryId,
            DueDate = todo.DueDate,
            Label = todo.Label,
            Latitude = todo.Latitude,
            Longitude = todo.Longitude
        });
        if (!response.IsSuccessStatusCode) {
            throw new Exception(response.StatusCode switch {
                System.Net.HttpStatusCode.BadRequest => "la tâche n'est pas enregistrable",
                System.Net.HttpStatusCode.Unauthorized => "il faut se connecter pour enregistrer",
                System.Net.HttpStatusCode.Forbidden => "pas le droit d'enregistrer",
                _ => "merci de réessayer un peu plus tard"
            });
        }
        var dto = await response.Content.ReadFromJsonAsync<TodoDetailDto>();
        _cacheTodos = null;
        if (dto is null) return null;
        return ToModel(dto);
    }

    private TimeSpan _maxCache = TimeSpan.FromSeconds(30);
    private (List<Todo> data, DateTime timestamp)? _cacheTodos;
    private (List<Category> data, DateTime timestamp)? _cacheCategories;
}
