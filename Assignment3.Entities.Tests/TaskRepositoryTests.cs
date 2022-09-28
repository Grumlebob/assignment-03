using Assignment3.Core;
using Microsoft.Data.Sqlite;

namespace Assignment3.Entities.Tests;


public sealed class TaskRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        
        context.Users.AddRange(new User() { Id = 1 , Name = "John", Email = "John@example.com" }, new User() { Id = 2 , Name = "Bob", Email = "Bob@example.com" });
        context.SaveChanges();

        _context = context;
        _repository = new TaskRepository(_context);
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}

