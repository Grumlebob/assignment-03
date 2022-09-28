namespace Assignment3.Entities.Tests;

public class TagRepositoryTests
{
    public readonly DbContextOptions<KanbanContext> dbContextOptions;

    public TagRepositoryTests()
    {
        // Build DbContextOptions
        dbContextOptions = new DbContextOptionsBuilder<KanbanContext>()
            .UseInMemoryDatabase(databaseName: "InMemDB")
            .Options;
    }

    [Fact]
    public void update_or_delete_returns_null(){
        
    }
}
