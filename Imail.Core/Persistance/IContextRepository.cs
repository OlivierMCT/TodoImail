using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Imail.Core.Persistance; 
public interface IContextRepository<TEntity> where TEntity : BaseEntity {
    BaseDbContext Context { get; init; }

    Task<List<TEntity>> GetAll() => Context.Set<TEntity>().ToListAsync();

    Task<List<TEntity>> GetBy(Expression<Func<TEntity, bool>> filter) => Context.Set<TEntity>().Where(filter).ToListAsync();

    Task<TEntity?> GetById(int id) => Context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id);

    Task<TEntity?> GetByRowId(Guid rowId) => Context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.RowId == rowId);

    Task<bool> Exists(int id) => Context.Set<TEntity>().AnyAsync(entity => entity.Id == id);

    async Task Persist(TEntity entity) {
        var e = await GetById(entity.Id);
        if (e != null) Context.Entry(e).CurrentValues.SetValues(entity);
        else Context.Add(entity);
    }

    Task Delete(TEntity entity) {
        Context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    Task<int> SaveChanges() => Context.SaveChangesAsync();
}
