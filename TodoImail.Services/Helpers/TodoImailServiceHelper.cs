using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TodoImail.Services.Entities;
using TodoImail.Services.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoImail.Services.Helpers; 
public class TodoImailServiceHelper {
    public virtual Category ToCategoryModel(CategoryEntity entity, int? count) {
        return new() { 
            Id = entity.Id,
            TodosCount = count,
            Label = entity.Label,
            Color = ToColorModel(entity.Color)
        };
    }

    public virtual Color ToColorModel(string strColor) => Color.FromArgb(int.Parse(strColor));
    public virtual string ToColorEntity(Color color) => $"{color.ToArgb()}";

    public virtual Todo ToTodoModel(TodoEntity entity) {
        return new() {
            Id = entity.Id,
            Label = entity.Label,
            DueDate = entity.DueDate,
            IsDeletable = ToDeletableModel(entity),
            IsDone = entity.IsDone,
            Status = ToStatusModel(entity),
            Category = entity.Category != null ? ToCategoryModel(entity.Category, null) : null,
            Coords = ToCoordsModel(entity)
        };
    }

    public virtual bool ToDeletableModel(TodoEntity entity) {
        return entity.IsDone && entity.DueDate < DateOnly.FromDateTime(DateTime.Today);
    }

    public virtual TodoStatus ToStatusModel(TodoEntity entity) {
        TodoStatus status;
        if (entity.IsDone) {
            status = entity.DueDate < DateOnly.FromDateTime(DateTime.Today.AddMonths(-1)) ? TodoStatus.Archived : TodoStatus.Closed;
        } else {
            status = entity.DueDate < DateOnly.FromDateTime(DateTime.Today) ? TodoStatus.Late : (entity.DueDate < DateOnly.FromDateTime(DateTime.Today.AddDays(2)) ? TodoStatus.Urgent : TodoStatus.Pending);
        }
        return status;
    }

    public virtual TodoCoordinate? ToCoordsModel(TodoEntity entity) {
        return entity.Latitude.HasValue && entity.Longitude.HasValue ?
            new TodoCoordinate() { Latitude = entity.Latitude.Value, Longitude = entity.Longitude.Value } : null;
    }

    public virtual void ValidateData(object obj) {
        List<ValidationResult> results = [];
        if (!Validator.TryValidateObject(obj, new(obj), results, true)) {
            var errors = results.Select(vr => $"{vr.MemberNames.First()}:{vr.ErrorMessage}");
            throw new TodoImailServiceException(4, string.Join("|", errors));
        }
    }

    public virtual TodoEntity ToTodoEntity(TodoCreate todo) {
        ValidateData(todo); 
        return new() {
            Label = todo.Label!,
            DueDate = todo.DueDate!.Value,
            IsDone = false,
            CategoryId = todo.CategoryId,
            Latitude = todo.Latitude,
            Longitude = todo.Longitude,            
        };
    }

    public virtual TodoEntity ToTodoEntity(TodoModify todo) {
        ValidateData(todo); 
        return new() {
            Id = todo.Id!.Value,
            Label = todo.Label!,
            DueDate = todo.DueDate!.Value,
            IsDone = todo.IsDone!.Value,
            CategoryId = todo.CategoryId,
            Latitude = todo.Latitude,
            Longitude = todo.Longitude,
        };
    }

}
