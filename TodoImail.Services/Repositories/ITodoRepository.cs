using Imail.Core.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoImail.Services.DbContexts;
using TodoImail.Services.Entities;

namespace TodoImail.Services.Repositories; 
public interface ITodoRepository : IContextRepository<TodoEntity> {
    new Task<List<TodoEntity>> GetAll() => Context.Set<TodoEntity>().Include(entity => entity.Category).ToListAsync();
    new Task<List<TodoEntity>> GetBy(Expression<Func<TodoEntity, bool>> filter) => Context.Set<TodoEntity>().Include(entity => entity.Category).Where(filter).ToListAsync();
    new Task<TodoEntity?> GetById(int id) => Context.Set<TodoEntity>().Include(entity => entity.Category).FirstOrDefaultAsync(entity => entity.Id == id);
    new Task<TodoEntity?> GetByRowId(Guid rowId) => Context.Set<TodoEntity>().Include(entity => entity.Category).FirstOrDefaultAsync(entity => entity.RowId == rowId);
}

public class TodoRepository(TodoImailDbContext context) : ITodoRepository {
    BaseDbContext IContextRepository<TodoEntity>.Context { get; init; } = context;

    public ITodoRepository AsITodoRepository() {  return this; }
    public TodoImailDbContext Context { get  => (TodoImailDbContext)this.AsITodoRepository().Context; }
}