using Assignment3.Core;

namespace Assignment3.Entities.Tests;

public class UserRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly UserRepository _userRepository;

    //Prøvet en philip route.
    public UserRepositoryTests()
    {
        // Build DbContextOptions
        var context = new DbContextOptionsBuilder<KanbanContext>()
            .UseInMemoryDatabase(databaseName: "InMemDB")
            .Options;
        
        _context = new KanbanContext(context);
        _userRepository = new UserRepository(_context);

    }

    [Fact]
    public void Test1()
    {
        // Arrange
        UserCreateDTO userCreateDTO = new UserCreateDTO(Email: "TestMail", Name: "TestName");
        
        // Act
        var (response, userId) = _userRepository.Create(userCreateDTO);
        
        // Assert
        Assert.Equal(Response.Created, response);
        Assert.Equal(1, userId);
        Assert.Equal(1, _context.Users.Count());
        
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
