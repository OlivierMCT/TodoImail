using System.ComponentModel.DataAnnotations;
using TodoImail.Services.Helpers;
using TodoImail.Services.Models;
using TodoImail.Services.Repositories;

namespace TodoImail.Services.Services; 
public interface ITodoImailService {
    Task<List<Category>> ListCategories();
    Task<Category> GetCategory(int id);
    Task<Category> GetCategory(string label);

    Task<List<Todo>> ListTodos(int? categoryId = null);
    Task<List<Todo>> SearchTodos(string keyword);
    Task<Todo> GetTodo(int id);

    Task<Todo> CreateTodo(TodoCreate data);
    Task<Todo> ModifyTodo(TodoModify data);
    Task<Todo> RemoveTodo(int id);
    Task<Todo> ToggleTodo(int id);
}

public class TodoImailServiceDefaultImplementation(ITodoRepository todos, ICategoryRepository categories) : ITodoImailService {
    private ITodoRepository _todos = todos;
    private ICategoryRepository _categories = categories;

    public TodoImailServiceHelper Helper { get; init; } = new TodoImailServiceHelper();

    public async Task<List<Category>> ListCategories() {
        var entities = await _categories.GetAll();
        List<Category> models = [];
        entities.ToList().ForEach(e => models.Add(Helper.ToCategoryModel(e, _categories.GetTodoCount(e.Id).Result)));
        return models;
    }

    public async Task<Category> GetCategory(int id) {
        var entity = await _categories.GetById(id);
        if (entity == null) throw new TodoImailServiceException(1, $"category #{id} not found");

        return Helper.ToCategoryModel(entity, await _categories.GetTodoCount(id));
    }

    public async Task<Category> GetCategory(string label) {
        var entity = (await _categories.GetBy(c => c.Label.ToLower() == label.ToLower())).FirstOrDefault() ?? throw new TodoImailServiceException(1, $"category '{label}' not found");
        return Helper.ToCategoryModel(entity, await _categories.GetTodoCount(entity.Id));
    }

    public async Task<List<Todo>> ListTodos(int? categoryId = null) {
        if (categoryId.HasValue && ! (await _categories.Exists(categoryId.Value))) 
            throw new TodoImailServiceException(1, $"category #{categoryId} not found");
        var entities = await (categoryId is null ? _todos.GetAll() : _todos.GetBy(t => t.CategoryId == categoryId));
        return [.. entities.Select(Helper.ToTodoModel)];
    }

    public async Task<List<Todo>> SearchTodos(string keyword) {
        var entities = await _todos.GetBy(t => t.Label.Contains(keyword));
        return [.. entities.Select(Helper.ToTodoModel)];
    }

    public async Task<Todo> GetTodo(int id) {
        var entity = await _todos.GetById(id) ?? throw new TodoImailServiceException(2, $"todo #{id} not found");
        return Helper.ToTodoModel(entity);
    }

    public async Task<Todo> CreateTodo(TodoCreate data) {
        var entity = Helper.ToTodoEntity(data);

        if (entity.CategoryId.HasValue && await _categories.GetById(entity.CategoryId.Value) is null)
            throw new TodoImailServiceException(1, $"category #{entity.CategoryId} not found");

        await _todos.Persist(entity);
        await _todos.SaveChanges();
        return Helper.ToTodoModel(entity);
    }

    public async Task<Todo> ModifyTodo(TodoModify data) {        
        var entity = Helper.ToTodoEntity(data);

        if (entity.CategoryId.HasValue && !(await _categories.Exists(entity.CategoryId.Value)))
            throw new TodoImailServiceException(1, $"category #{entity.CategoryId} not found");

        if (! await _todos.Exists(data.Id!.Value))
            throw new TodoImailServiceException(2, $"todo #{data.Id} not found");

        await _todos.Persist(entity);
        await _todos.SaveChanges();
        return Helper.ToTodoModel(entity);
    }

    public async Task<Todo> RemoveTodo(int id) {
        var entity = await _todos.GetById(id) ?? throw new TodoImailServiceException(2, $"todo #{id} not found");
        var model = Helper.ToTodoModel(entity);
        if(!model.IsDeletable) throw new TodoImailServiceException(3, $"todo #{id} is not deletable");
        await _todos.Delete(entity);
        await _todos.SaveChanges();
        return model;
    }

    public async Task<Todo> ToggleTodo(int id) {
        var entity = await _todos.GetById(id) ?? throw new TodoImailServiceException(2, $"todo #{id} not found");
        entity.IsDone = !entity.IsDone;
        await _todos.Persist(entity);
        await _todos.SaveChanges();
        return Helper.ToTodoModel(entity);
    }
}
