using Imail.Core.Persistance;
using Microsoft.EntityFrameworkCore;
using TodoImail.Services.DbContexts;
using TodoImail.Services.Entities;

namespace TodoImail.Services.Repositories;
public interface ICategoryRepository : IContextRepository<CategoryEntity> {
    Task<int> GetTodoCount(int categoryId) => Context.Set<CategoryEntity>().Where(e => e.Id == categoryId).SumAsync(c => c.Todos.Count);
}

public class CategoryRepository(TodoImailDbContext context) : ICategoryRepository {
    BaseDbContext IContextRepository<CategoryEntity>.Context { get; init; } = context;

    public ICategoryRepository AsICategoryRepository() { return this; }
    public TodoImailDbContext Context { get => (TodoImailDbContext)this.AsICategoryRepository().Context; }
}
