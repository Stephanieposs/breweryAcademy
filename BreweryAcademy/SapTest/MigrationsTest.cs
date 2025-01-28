using Microsoft.EntityFrameworkCore;
using SAP4;
using Microsoft.Data.Sqlite;

namespace SapTest
{
    public class MigrationsTest
    {
        [Fact]
        public void TestMigrations()
        {
            // Arrange: Use SQLite in-memory database
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseSqlite(connection) // Use SQLite in-memory
                .Options;

            using (var context = new DefaultContext(options))
            {
                // Ensure the database is created
                context.Database.EnsureCreated();

                // Act: Simulate ApplyMigrations logic
                var pendingMigrations = context.Database.GetPendingMigrations().ToList();
                Assert.NotEmpty(pendingMigrations); // Ensure there are pending migrations
                
                context.Database.Migrate(); // Apply migrations

                // Assert: Verify no pending migrations remain
                pendingMigrations = context.Database.GetPendingMigrations().ToList();
                Assert.Empty(pendingMigrations);
            }

            connection.Close();
        }

    }
}