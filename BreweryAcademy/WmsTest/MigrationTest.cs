using Microsoft.EntityFrameworkCore;
using WMS.Data;

namespace Test
{
    public class MigrationTest
    {
        [Fact]
        public void Test_Migration_Applied_Successfully()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseSqlite("DataSource=:memory:") //memory DB
                .Options;

            using (var context = new DefaultContext(options))
            {
                //Act
                context.Database.OpenConnection();
                context.Database.Migrate();

                var pendingMigrations = context.Database.GetPendingMigrations();
                var appliedMigrations = context.Database.GetAppliedMigrations();

                //Assert
                Assert.Empty(pendingMigrations);
                Assert.True(appliedMigrations.Any());
            }
        }
    }
}