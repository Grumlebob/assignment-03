using Assignment3.Core;
using Microsoft.Data.Sqlite;
using static Assignment3.Core.Response;

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
        
        var startTask = new Task
        {
            Id = 1,
            Title = "Start",
            Description = "Start",
            State = State.New,
            AssignedTo = new User("Start"),
            StateUpdated = DateTime.Now,
            Tags = new List<Tag>() {new Tag("StartTagOne"), new Tag("StartTagTwo")}
        };
        
        var startTaskTwo = new Task
        {
            Id = 2,
            Title = "StartTwo",
            Description = "StartTwo",
            State = State.New,
            AssignedTo = new User("StartTwo"),
            StateUpdated = DateTime.Now,
            Tags = new List<Tag>() {new Tag("StartTagOneTwo"), new Tag("StartTagTwoTwo")}
        };
        context.Tasks.AddRange(startTask, startTaskTwo);
        context.SaveChanges();

        _connection = connection;
        _context = context;
        _repository = new TaskRepository(_context);
    }
    
    
    [Fact]
    public void Create()
    {
        //TaskCreateDTO ([StringLength(100)] string Title, int? AssignedToId, string? Description, ICollection<string> Tags);
        var task = new TaskCreateDTO(Title: "Third",AssignedToId:1, Description: "Third", Tags: new List<string>() {"ThirdTagOne", "ThirdTagTwo"});

        var expected = new TaskDetailsDTO(3, "Third", "Third", DateTime.Now, new UserDTO(1, "Start",null).Name, new List<string>() {"ThirdTagOne", "ThirdTagTwo"}, State.New, DateTime.Now);

        var (status, created) = _repository.Create(task);
        
        var expectedTime = DateTime.Now;
        
        expectedTime.Should().BeCloseTo(expected.StateUpdated, precision: TimeSpan.FromMinutes(5));
        status.Should().Be(Created);
        created.Should().Be(expected.Id);
    }

    [Fact]
    public void Find()
    {
        var Task = _repository.Find(1);
        
        var expected = new TaskDetailsDTO(1, "Start", "Start", DateTime.Now, new UserDTO(1, "Start",null).Name, new List<string>() {"StartTagOne", "StartTagTwo"}, State.New, DateTime.Now);
        
        var expectedTime = DateTime.Now;
        
        expectedTime.Should().BeCloseTo(expected.StateUpdated, precision: TimeSpan.FromMinutes(5));
        Task.Id.Should().Be(expected.Id);
        Task.Title.Should().Be(expected.Title);
        Task.Description.Should().Be(expected.Description);
    }

    [Fact]
    public void Find_Non_Existing() => _repository.Find(42).Should().BeNull();

    [Fact]
    public void Read()
    {
        var characters = _repository.Read();

        //int Id, string Title, string AssignedToName, IReadOnlyCollection<string> Tags, State State)
        var expected = new TaskDTO[] {
            new TaskDTO(1, "Start", "Start", new List<string>() {new string("StartTagOne"), new string("StartTagTwo")}, State.New),
            new TaskDTO(2, "StartTwo", "StartTwo", new List<string>() {new string("StartTagOneTwo"), new string("StartTagTwoTwo")}, State.New)
        };

        characters.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Update()
    {
        //(int Id, [StringLength(100)] string Title, int? AssignedToId, string? Description, ICollection<string> Tags, State State);
        var task = new TaskUpdateDTO(Id: 1, Title: "Updated", AssignedToId: 1, Description: "Updated", Tags: new List<string>() {"UpdatedTagOne", "UpdatedTagTwo"}, State: State.Active);
        var expected = new TaskDetailsDTO(1, "Updated", "Updated", DateTime.Now, new UserDTO(1, "Start",null).Name, new List<string>() {"UpdatedTagOne", "UpdatedTagTwo"}, State.Active, DateTime.Now);

        var status = _repository.Update(task);

        status.Should().Be(Updated);
        var rep = _repository.Find(1);
        rep.Title.Should().Be(expected.Title);
        rep.Description.Should().Be(expected.Description);
        rep.Tags.Should().BeEquivalentTo(expected.Tags);
    }

    [Fact]
    public void Update_Non_Existing()
    {
        var character = new TaskUpdateDTO(Id: 42, Title: "Updated", AssignedToId: 1, Description: "Updated", Tags: new List<string>() {"UpdatedTagOne", "UpdatedTagTwo"}, State: State.Active);
        var status = _repository.Update(character);

        status.Should().Be(NotFound);
    }

    [Fact]
    public void Delete()
    {
        var status = _repository.Delete(1);

        status.Should().Be(Deleted);
        _context.Tasks.Find(1).Should().BeNull();
    }

    [Fact]
    public void Delete_Non_Existing() => _repository.Delete(42).Should().Be(NotFound);

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

