using Assignment3.Core;
using Microsoft.Data.Sqlite;
using static Assignment3.Core.Response;

namespace Assignment3.Entities.Tests;


public sealed class TagRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repository;
    private readonly SqliteConnection _connection;

    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Add(new Tag("StartTestTag"));
        context.SaveChanges();

        _connection = connection;
        _context = context;
        _repository = new TagRepository(_context);
    }
 
    [Fact]
    public void update_returns_update_or_notFound(){
        _repository.Create(new TagCreateDTO(Name: "test"));   

        var tagTwo = new TagUpdateDTO(2, "tested");
        var actual =_repository.Update(tagTwo); 

        actual.Should().Be(Updated);
    }
    [Fact]
    public void delete_without_force_returns_conflict(){
        _repository.Create(new TagCreateDTO(Name: "test"));
        
        var actual = _repository.Delete(2, false);

        actual.Should().Be(Conflict);
    }
    [Fact]
    public void delete_with_force_deletes(){
        var temp = new TagCreateDTO(Name: "test");
        _repository.Create(temp);

        var actual = _repository.Delete(1, true);

        actual.Should().Be(Deleted);
    }
    [Fact]
    public void create_already_created_returns_conflict(){
        _repository.Create(new TagCreateDTO(Name: "test"));
        
        var (actual, resposne) = _repository.Create(new TagCreateDTO(Name: "test"));

        actual.Should().Be(Conflict);
    }

    [Fact]
    public void CreateTest()
    {
        var tag = new TagCreateDTO("Test");
        
        var (status, created) = _repository.Create(tag);

        status.Should().Be(Created);
        created.Should().Be(2);
        _repository.Find(2).Should().BeEquivalentTo(tag);
    }
    [Fact]
    public void FindTest()
    {
        var tag = new TagCreateDTO("TestTag");
        var (status, created) = _repository.Create(tag);
        
        _repository.Find(2).Should().Be(new TagDTO(2, "TestTag"));
    }
    
    [Fact]
    public void FindTestNull()
    {
        var tag = new TagCreateDTO("TestTag");
        var (status, created) = _repository.Create(tag);
        
        _repository.Find(10).Should().BeNull();
    }

    [Fact]
    public void Read()
    {
        var tag1 = new TagCreateDTO("Doing");
        _repository.Create(tag1);
        var tag2 = new TagCreateDTO("Smoking");
        _repository.Create(tag2);
        
        _repository.Read().Should().BeEquivalentTo(new[] {new TagDTO(1, "StartTestTag"), new TagDTO(2, "Doing"), new TagDTO(3, "Smoking") });

    }
    
    [Fact]
    public void Update_Non_Existing()
    {
        
        _repository.Update(new TagUpdateDTO(42, "Central City")).Should().Be(NotFound);
    }
    

    [Fact]
    public void Update()
    {
        var response = _repository.Update(new TagUpdateDTO(1, "Yay, new name"));

        response.Should().Be(Updated);

        var entity = _context.Tags.Find(1)!;

        entity.Name.Should().Be("Yay, new name");
    }

    [Fact]
    public void Delete_Non_Existing() => _repository.Delete(42,true).Should().Be(NotFound);

    [Fact]
    public void Delete()
    {
        var response = _repository.Delete(1, true);

        response.Should().Be(Deleted);

        var entity = _context.Tags.Find(1);

        entity.Should().BeNull();
    }

    [Fact]
    public void Delete_Conflict()
    {
        var response = _repository.Delete(1, false);

        response.Should().Be(Conflict);

        _context.Tags.Find(1).Should().NotBeNull();
    }
    
    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
