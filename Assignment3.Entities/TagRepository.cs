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

        var newTag = new Tag();
        
        
            newTag.Name = tag.Name;
            var id = 0;
            try{    
            id = context.Tags.Where(x => x.Name == tag.Name).First().Id;
            }catch
            {

            }
            if (context.Tags.Find(id) != null) return (Response.Conflict, newTag.Id);
            else{
            context.Tags.Add(newTag);
            context.SaveChanges();
            return (Response.Created, newTag.Id);
            }
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
        try
        {
            if (context.Tags.Find(tagId)!.Tasks != null && !force) return Response.Conflict;
            else
            {
                var newTag = context.Tags.Where(t => t.Id == tagId).First();
                context.Remove(newTag);
                context.SaveChanges();
                return Response.Deleted;
            }
        }
        catch
        {
            return Response.NotFound;
        }
    }
}