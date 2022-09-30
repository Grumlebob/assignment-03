using Assignment3.Core;
using Microsoft.Data.Sqlite;

namespace Assignment3.Entities.Tests;


public sealed class TaskRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repository;
    private readonly SqliteConnection _connection;

    public TaskRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.SaveChanges();

        _connection = connection;
        _context = context;
        _repository = new TaskRepository(_context);
    }
    [Fact]
    public void delete_active_resolved_closed_removed_returns_conflict()
    {
        _context.Users.AddRange(new User() { Id = 1 , Name = "test", Email = "test@mail.dk" });
        var task = new TaskCreateDTO(Title: "test", AssignedToId: 1, Description: "testing", Tags: null!);
        var taskTwo = new TaskCreateDTO(Title: "testTwo", AssignedToId: 1, Description: "testing", Tags: null!);
        var taskThree = new TaskCreateDTO(Title: "testThree", AssignedToId: 1, Description: "testing", Tags: null!);
        var taskFour = new TaskCreateDTO(Title: "testFour", AssignedToId: 1, Description: "testing", Tags: null!);

        _repository.Create(task);
        _repository.Create(taskTwo);
        _repository.Create(taskThree);
        _repository.Create(taskThree);

        _repository.Update(new TaskUpdateDTO(Id: 1, Title: "test", AssignedToId: 1, Description: "testing", Tags: null!, State: State.Active));
        _repository.Update(new TaskUpdateDTO(Id: 2, Title: "testTwo", AssignedToId: 1, Description: "testing", Tags: null!, State: State.Resolved));
        _repository.Update(new TaskUpdateDTO(Id: 3, Title: "testThree", AssignedToId: 1, Description: "testing", Tags: null!, State: State.Closed));
        _repository.Update(new TaskUpdateDTO(Id: 4, Title: "testFour", AssignedToId: 1, Description: "testing", Tags: null!, State: State.Removed));

        var actual = _repository.Delete(1);
        var actualTwo = _repository.Delete(2);
        var actualThree = _repository.Delete(3);
        var actualFour = _repository.Delete(4);

        actual.Should().Be(Response.Conflict);
        actualTwo.Should().Be(Response.Conflict);
        actualThree.Should().Be(Response.Conflict);
        actualFour.Should().Be(Response.Conflict);
    }
    [Fact]
    public void create_sets_stateupdated_to_now(){
        _context.Users.AddRange(new User() { Id = 1, Name = "test", Email = "test@mail.dk" });
        var (reponse, id) = _repository.Create(new TaskCreateDTO(Title: "test", AssignedToId: 1, Description: "testing", Tags: null!));
        var actual = _context.Tasks.Find(id)!.StateUpdated.AddSeconds(2);

        var expected = DateTime.Now;

        actual.Should().BeCloseTo(expected, precision: TimeSpan.FromSeconds(5));
    }
    [Fact]
    public void update_sets_stateupdated_to_now(){
        _context.Users.AddRange(new User() { Id = 1, Name = "test", Email = "test@mail.dk" });
        _repository.Create(new TaskCreateDTO(Title: "test", AssignedToId: 1, Description: "testing", Tags: null!));
        _repository.Update(new TaskUpdateDTO(Id: 1, Title: "test", AssignedToId: 1, Description: "testing", Tags: null!, State: State.Active));
        var actual = _context.Tasks.Find(1)!.StateUpdated.AddSeconds(2);

        var expected = DateTime.Now;

        actual.Should().BeCloseTo(expected, precision: TimeSpan.FromSeconds(5));
    }
    [Fact]
    public void assigning_non_existing_user_returns_bad_request(){
        var (actual, id) = _repository.Create(new TaskCreateDTO(Title: "test", AssignedToId: 1, Description: "testing", Tags: null!));

        actual.Should().Be(Response.BadRequest);
    }
    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

