using Imail.Core.Persistance;
using Microsoft.EntityFrameworkCore;
using TodoImail.Services.Entities;

namespace TodoImail.Services.DbContexts; 
public class TodoImailDbContext(DbContextOptions options) : BaseDbContext(options) {
    public DbSet<TodoEntity>  Todos { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<TodoEntity>(e => {
            e.Property(t => t.Label).HasMaxLength(255);  
        });

        modelBuilder.Entity<CategoryEntity>(e => {
            e.Property(c => c.Label).HasMaxLength(50);
            e.HasIndex(c => c.Label);
            e.HasIndex(c => new { c.Label, c.Color }).IsUnique();
            e.Property(c => c.Color).HasMaxLength(15);
        });
    }
}
