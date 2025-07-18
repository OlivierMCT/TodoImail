
using TodoImail.Services.Models;
using TodoImail.WebApi.Contracts.Dtos;

namespace TodoImail.WebApi.Helpers;
public class ControllerHelper {
    virtual public CategoryDto ToCategoryDto(Category model) {
        return new CategoryDto() {
            Id = model.Id,
            Label = model.Label,
            TodosCount = model.TodosCount,
            Color = "#" + model.Color.Name
        };
    }

    virtual public TodoDetailDto ToTodoDetailDto(Todo model) {
        return new TodoDetailDto() {
            Id = model.Id,
            Label = model.Label,
            Category = model.Category is null ? null : ToCategoryDto(model.Category),
            DueDate = model.DueDate,
            IsDeletable = model.IsDeletable,
            IsDone  = model.IsDone,
            Status = model.Status.ToString(),
            Latitude = model.Coords?.Latitude,
            Longitude = model.Coords?.Longitude,
        };
    }

    virtual public TodoDto ToTodoDto(Todo model) {
        return new TodoDto() {
            Id = model.Id,
            Label = model.Label,
            CategoryId = model.Category?.Id,
            IsDone = model.IsDone,
            Status = model.Status.ToString(),
        };
    }

    virtual public TodoCreate ToTodoCreate(TodoPostDto dto) {
        return new TodoCreate() {
            Label = dto.Label,
            CategoryId = dto.CategoryId,
            DueDate = dto.DueDate,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };
    }

    virtual public TodoModify ToTodoModify(int id, TodoPutDto dto) {
        return new TodoModify() {
            Id = id,
            Label = dto.Label,
            CategoryId = dto.CategoryId,
            DueDate = dto.DueDate,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            IsDone = dto.IsDone,
        };
    }
}
