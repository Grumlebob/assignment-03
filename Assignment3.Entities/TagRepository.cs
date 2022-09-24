using Assignment3.Core;

namespace Assignment3.Entities;

public class TagRepository : ITagRepository
{
    private readonly KanbanContext context;
    
    public TagRepository(KanbanContext context)
    {
        this.context = context;
    }
    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        //Bare test eksempel uden at have fulgt opgave kravene.
        var response = Response.Created;
        
        var newTag =  new Tag{
            Name = tag.Name,
        };
        
        context.Tags.Add(newTag);
        context.SaveChanges();
        return (response, newTag.Id);
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