using Assignment3.Core;
using Microsoft.Data.Sqlite;

namespace Assignment3.Entities.Tests;

public sealed class UserRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }
    [Fact]
    public void delete_with_force_deletes()
    {
        _repository.Create(new UserCreateDTO(Name: "test", Email: "test@mail.dk"));

        var actual = _repository.Delete(1, true);

        actual.Should().Be(Response.Deleted);
    }
    [Fact]
    public void delete_without_force_returns_conflict()
    {
        _repository.Create(new UserCreateDTO(Name: "test", Email: "test@mail.dk"));

        var actual = _repository.Delete(1, false);

        actual.Should().Be(Response.Conflict);
    }
    [Fact]
    public void create_alrdy_created_email_returns_conflict()
    {
        _repository.Create(new UserCreateDTO(Name: "test", Email: "test@mail.dk"));
        var (response, id) = _repository.Create(new UserCreateDTO(Name: "test", Email: "test@mail.dk"));

        response.Should().Be(Response.Conflict);
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
