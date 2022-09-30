using Assignment3.Core;
using Microsoft.Data.Sqlite;
using static Assignment3.Core.Response;

namespace Assignment3.Entities.Tests;

public sealed class UserRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly UserRepository _repository;
    private readonly SqliteConnection _connection;

    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Add(new User("StartTestUser"));
        context.SaveChanges();

        _connection = connection;
        _context = context;
        _repository = new UserRepository(_context);
    }
    [Fact]
    public void delete_with_force_deletes()
    {
        _repository.Create(new UserCreateDTO(Name: "test", Email: "test@mail.dk"));

        var actual = _repository.Delete(1, true);

        actual.Should().Be(Deleted);
    }
    [Fact]
    public void delete_without_force_returns_conflict()
    {
        _repository.Create(new UserCreateDTO(Name: "test", Email: "test@mail.dk"));

        var actual = _repository.Delete(1, false);

        actual.Should().Be(Conflict);
    }
    [Fact]
    public void create_alrdy_created_email_returns_conflict()
    {
        _repository.Create(new UserCreateDTO(Name: "test", Email: "test@mail.dk"));
        var (response, id) = _repository.Create(new UserCreateDTO(Name: "test", Email: "test@mail.dk"));

        response.Should().Be(Conflict);
    }
    
    
    [Fact]
    public void CreateTest()
    {
        var user = new UserCreateDTO("Test",null);
        
        var (status, created) = _repository.Create(user);

        status.Should().Be(Created);
        created.Should().Be(2);
        _repository.Find(2).Should().BeEquivalentTo(user);
    }
    [Fact]
    public void FindTest()
    {
        var user = new UserCreateDTO("Testuser",null);
        var (status, created) = _repository.Create(user);
        
        _repository.Find(2).Should().Be(new UserDTO(2, "Testuser",null));
    }
    
    [Fact]
    public void FindTestNull()
    {
        var user = new UserCreateDTO("Testuser","TestEmail");
        var (status, created) = _repository.Create(user);
        
        _repository.Find(10).Should().BeNull();
    }

    [Fact]
    public void Read()
    {
        var user1 = new UserCreateDTO("Doing",null);
        _repository.Create(user1);
        var user2 = new UserCreateDTO("Smoking",null);
        _repository.Create(user2);
        
        _repository.Read().Should().BeEquivalentTo(new[] {new UserDTO(1, "StartTestUser",null), new UserDTO(2, "Doing",null), new UserDTO(3, "Smoking",null) });

    }
    
    [Fact]
    public void Update_Non_Existing()
    {
        _repository.Update(new UserUpdateDTO(42, "None","None")).Should().Be(NotFound);
    }
    

    [Fact]
    public void Update()
    {
        var response = _repository.Update(new UserUpdateDTO(1, "Yay, new name",null));

        response.Should().Be(Updated);

        var entity = _context.Users.Find(1)!;

        entity.Name.Should().Be("Yay, new name");
        entity.Email.Should().BeNull();
        entity.Id.Should().Be(1);
    }

    [Fact]
    public void Delete_Non_Existing() => _repository.Delete(42,true).Should().Be(NotFound);

    [Fact]
    public void Delete()
    {
        var response = _repository.Delete(1, true);

        response.Should().Be(Deleted);

        var entity = _context.Users.Find(1);

        entity.Should().BeNull();
    }

    [Fact]
    public void Delete_Conflict()
    {
        var response = _repository.Delete(1, false);

        response.Should().Be(Conflict);

        _context.Users.Find(1).Should().NotBeNull();
    }
    
    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
