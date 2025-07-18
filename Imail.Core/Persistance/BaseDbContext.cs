using Microsoft.EntityFrameworkCore;

namespace Imail.Core.Persistance; 
public abstract class BaseDbContext : DbContext {
    public BaseDbContext(DbContextOptions options) : base(options) {
        SavingChanges += BaseDbContext_SavingChanges;
    }

    private void BaseDbContext_SavingChanges(object? sender, SavingChangesEventArgs e) {
        ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified)
            .ToList()
            .ForEach(e => {
                if (e.State is EntityState.Added) {
                    e.Entity.RowId = Guid.NewGuid();
                    e.Entity.CreatedAt = DateTime.Now;
                } else if (e.State is EntityState.Modified) {
                    e.Entity.UpdatedAt = DateTime.Now;
                }

            });
    }
}
