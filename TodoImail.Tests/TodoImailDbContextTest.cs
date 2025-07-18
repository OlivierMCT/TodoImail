using Microsoft.EntityFrameworkCore;
using TodoImail.Services.DbContexts;
using TodoImail.Services.Entities;

namespace TodoImail.Tests {
    [TestClass]
    public sealed class TodoImailDbContextTest {
        [TestMethod]
        public void CreationDeLaBase() {
            DbContextOptionsBuilder builder = new ();
            builder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TodoImailDbContextTest_CreationDeLaBase;Integrated Security=True");
            TodoImailDbContext context = new(builder.Options);
            context.Database.EnsureDeleted();

            context.Database.EnsureCreated();
        }

        [TestMethod]
        public void GenerationALInsertion() {
            // Arrange
            DbContextOptionsBuilder builder = new();
            builder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TodoImailDbContextTest_GenerationALInsertion;Integrated Security=True");
            TodoImailDbContext context = new(builder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            DateTime before = DateTime.Now;
            TodoEntity entity = new() { DueDate = DateOnly.MinValue, IsDone = false, Label = "" };

            // Act
            context.Add(entity);
            context.SaveChanges();

            // Assert
            DateTime after = DateTime.Now;
            TodoEntity? expected = context.Todos.FirstOrDefault();
            Assert.IsNotNull(expected);
            Assert.IsTrue(expected.CreatedAt >= before, $"createdAt {expected.CreatedAt} is not >= {before}");
            Assert.IsTrue(expected.CreatedAt <= after);
            Assert.IsTrue(expected.RowId != Guid.Empty);
            context.Database.EnsureDeleted();
        }
    }
}
