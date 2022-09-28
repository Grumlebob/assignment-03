using Microsoft.Data.Sqlite;

namespace Assignment3.Entities.Tests;


public sealed class TagRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repository;

    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        
        context.Users.AddRange(new User() { Id = 1 , Name = "John", Email = "John@example.com" }, new User() { Id = 2 , Name = "Bob", Email = "Bob@example.com" });
        //context.Characters.Add(new Character { Id = 1, AlterEgo = "Superman", CityId = 1 });
        context.SaveChanges();

        _context = context;
        //_repository = new CityRepository(_context);
    }


    public void Dispose()
    {
        _context.Dispose();
    }
}
