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

        var newTag = new Tag
        {
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
        try
        {
            var newTag = context.Tags.Where(t => t.Id == tag.Id).First();
            newTag.Name = tag.Name;
            context.Tags.Update(newTag);
            context.SaveChanges();
            return Response.Updated;
        }
        catch
        {
            return Response.NotFound;
        }
    }

    public Response Delete(int tagId, bool force = false)
    {
        if(!force) return Response.Conflict;
        try
        {
            var newTag = context.Tags.Where(t => t.Id == tagId).First();
            context.Remove(newTag);
            context.SaveChanges();
            return Response.Deleted;
        }
        catch
        {
            return Response.NotFound;
        }
    }
}