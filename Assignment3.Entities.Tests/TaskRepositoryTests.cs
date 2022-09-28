namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests
{
    public readonly DbContextOptions<KanbanContext> dbContextOptions;

    public TaskRepositoryTests()
    {
        // Build DbContextOptions
        dbContextOptions = new DbContextOptionsBuilder<KanbanContext>()
            .UseInMemoryDatabase(databaseName: "InMemDB")
            .Options;
    }
}
