using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Configuration;

namespace Assignment3.Core;

public class TagRepository : ITagRepository
{
    public void LetsGo()
    {
        /*
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
        var connectionString = configuration.GetConnectionString("ConnectionString");

        var optionsBuilder = new DbContextOptionsBuilder<Assignment3.Entities.KanbanContext>();
        
        optionsBuilder.UseNpgsql(connectionString);

        var options = optionsBuilder.Options;

        Console.Write("Enter query to search for: ");

        var input = Console.ReadLine();

        using var context = new KanbanContext(options);
        */
    }
    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        throw new NotImplementedException();
    }

    public TagDTO Read(int tagId)
    {
        throw new NotImplementedException();
    }

    public Response Update(TagUpdateDTO tag)
    {
        throw new NotImplementedException();
    }

    public Response Delete(int tagId, bool force = false)
    {
        throw new NotImplementedException();
    }
}