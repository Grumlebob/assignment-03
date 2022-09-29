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
        context.SaveChanges();

        _context = context;
        _repository = new TagRepository(_context);
    }
 
    [Fact]
    public void update_returns_update_or_notFound(){
        _repository.Create(new TagCreateDTO(Name: "test"));   

        var tudto = new TagUpdateDTO(1, "tested");
        var actual =_repository.Update(tudto); 

        actual.Should().Be(Response.Updated);
    }
    [Fact]
    public void delete_without_force_returns_conflict(){
        _repository.Create(new TagCreateDTO(Name: "test"));


        var actual = _repository.Delete(1, false);

        actual.Should().Be(Response.Conflict);
    }
    [Fact]
    public void delete_with_force_deletes(){
        var temp = new TagCreateDTO(Name: "test");
        _repository.Create(temp);

        var actual = _repository.Delete(1, true);

        actual.Should().Be(Response.Deleted);
    }
    [Fact]
    public void create_already_created_returns_conflict(){
        _repository.Create(new TagCreateDTO(Name: "test"));
        
        var actual = _repository.Create(new TagCreateDTO(Name: "test"));

        actual.Should().Be((Response.Conflict, 0));
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
