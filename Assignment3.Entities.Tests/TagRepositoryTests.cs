using Assignment3.Core;
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
        _repository = new TagRepository(_context);
    }
    
    [Fact]
    public void CreateTag()
    {
        var (response, city) = _repository.Create(new TagCreateDTO(Name: "Bob"));
        
        Assert.Equal(1, _context.Tags.Count());
        Assert.Equal("Bob", _context.Tags.First().Name);
        Assert.Equal(1, _context.Tags.First().Id);
        //comment
    }
    [Fact]
    public void update_returns_update_or_notFound(){
        _repository.Create(new TagCreateDTO(Name: "bob"));
        

        var tudto = new TagUpdateDTO(1, "tested");
        var actual =_repository.Update(tudto); 

        actual.Should().Be(Response.Updated);
    }
    [Fact]
    public void delete_without_force_returns_conflict(){
        _repository.Create(new TagCreateDTO(Name: "bob"));

        var actual = _repository.Delete(1, false);

        actual.Should().Be(Response.Conflict);
    }
    [Fact]
    public void delete_with_force_deletes(){
        var temp = new TagCreateDTO(Name: "To do list");
        _repository.Create(temp);

        var actual = _repository.Delete(1, true);

        actual.Should().Be(Response.Deleted);
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
