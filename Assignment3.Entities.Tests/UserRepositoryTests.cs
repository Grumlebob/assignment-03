namespace Assignment3.Entities.Tests;

public class UserRepositoryTests
{
    public readonly DbContextOptions<KanbanContext> dbContextOptions;

    public UserRepositoryTests()
    {
        // Build DbContextOptions
        dbContextOptions = new DbContextOptionsBuilder<KanbanContext>()
            .UseInMemoryDatabase(databaseName: "InMemDB")
            .Options;
    }
}
