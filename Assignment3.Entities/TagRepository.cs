using Assignment3.Core;
using static Assignment3.Core.Response;

namespace Assignment3.Entities;

public class TagRepository : ITagRepository
{
    private readonly KanbanContext _context;

    public TagRepository(KanbanContext context)
    {
        this._context = context;
    }

    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        var entity = _context.Tags.FirstOrDefault(c => c.Name == tag.Name);
        Response response;

        if (entity is null)
        {
            entity = new Tag(tag.Name);

            _context.Tags.Add(entity);
            _context.SaveChanges();

            response = Created;
        }
        else
        {
            response = Conflict;
        }

        var created = new TagDTO(entity.Id, entity.Name);

        return (response, created.Id);
    }

    public TagDTO Find(int tagId)
    {
        var tag = from c in _context.Tags
            where c.Id == tagId
            select new TagDTO(c.Id, c.Name);

        return tag.FirstOrDefault()!;
    }

    public IReadOnlyCollection<TagDTO> Read()
    {
        var cities = from c in _context.Tags
            orderby c.Id
            select new TagDTO(c.Id, c.Name);

        return cities.ToArray();
    }
    
    public Response Update(TagUpdateDTO tag)
    {
        var entity = _context.Tags.Find(tag.Id);
        Response status;

        if (entity is null)
        {
            status = NotFound;
        }
        else
        {
            entity.Name = tag.Name;
            _context.SaveChanges();
            status = Updated;
        }

        return status;
    }

    public Response Delete(int tagId, bool force)
    {
        var tag = _context.Tags.FirstOrDefault(c => c.Id == tagId);
        Response status;

        if (tag is null)
        {
            status = NotFound;
        }
        else if (!force)
        {
            status = Conflict;
        }

        else if (tag.Tasks.Any())
        {
            status = Conflict;
        }
        else
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();

            status = Deleted;
        }

        return status;
    }
}