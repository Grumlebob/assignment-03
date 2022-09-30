using System.Net;
using Assignment3.Core;
using static Assignment3.Core.Response;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private readonly KanbanContext _context;

    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var entity = new Task()
        {
            Title = task.Title,
            Description = task.Description,
            AssignedTo = CreateOrUpdateUser(task.AssignedToId),
            StateUpdated = DateTime.Now,
            State = State.New,
            Tags = CreateOrUpdateTags(task.Tags).ToList(),
        };

        _context.Tasks.Add(entity);
        _context.SaveChanges();

        var created = new TaskDetailsDTO(entity.Id, entity.Title, entity.Description, Created: DateTime.Now,
            entity.AssignedTo.Name, Tags: entity.Tags.Select(t => t.Name).ToList(), entity.State, entity.StateUpdated);
        return (Created, created.Id);
    }

    public TaskDetailsDTO Find(int taskId)
    {
        var Tasks = from c in _context.Tasks
            let tags = c.Tags.Select(p => p.Name).ToList()
            where c.Id == taskId
            select new TaskDetailsDTO(c.Id, c.Title, c.Description, DateTime.Now, c.AssignedTo.Name, tags, c.State,
                c.StateUpdated);

        return Tasks.FirstOrDefault();
    }


    public IReadOnlyCollection<TaskDTO> Read()
    {
        //(int Id, string Title, string AssignedToName, IReadOnlyCollection<string> Tags, State State);
        var Tasks = from c in _context.Tasks
            let tags = c.Tags.Select(p => p.Name).ToList()
            select new TaskDTO(c.Id, c.Title, c.AssignedTo.Name, tags, c.State);

        return Tasks.ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadRemoved()
    {
        var Tasks = from c in _context.Tasks
            let tags = c.Tags.Select(p => p.Name).ToList()
            where c.State == State.Removed == true
            select new TaskDTO(c.Id, c.Title, c.AssignedTo.Name, tags, c.State);

        return Tasks.ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadByTag(string tag)
    {
        var Tasks = from c in _context.Tasks
            let tags = c.Tags.Select(p => p.Name).ToList()
            where tags.Contains(tag)
            select new TaskDTO(c.Id, c.Title, c.AssignedTo.Name, tags, c.State);

        return Tasks.ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadByUser(int userId)
    {
        var Tasks = from c in _context.Tasks
            let tags = c.Tags.Select(p => p.Name).ToList()
            where c.UserID == userId
            select new TaskDTO(c.Id, c.Title, c.AssignedTo.Name, tags, c.State);

        return Tasks.ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadByState(State state)
    {
        var Tasks = from c in _context.Tasks
            let tags = c.Tags.Select(p => p.Name).ToList()
            where c.State == state
            select new TaskDTO(c.Id, c.Title, c.AssignedTo.Name, tags, c.State);
        return Tasks.ToList();
    }

    public Response Update(TaskUpdateDTO task)
    {
        var entity = _context.Tasks.Find(task.Id);

        if (entity == null)
        {
            return NotFound;
        }

        entity.Title = task.Title;
        entity.Description = task.Description;
        entity.AssignedTo = CreateOrUpdateUser(task.AssignedToId);
        entity.StateUpdated = DateTime.Now;
        entity.State = State.New;
        entity.Tags = CreateOrUpdateTags(task.Tags).ToList();

        _context.SaveChanges();

        return Updated;
    }

    public Response Delete(int taskId)
    {
        var entity = _context.Tasks.Find(taskId);

        if (entity == null)
        {
            return NotFound;
        }

        _context.Tasks.Remove(entity);
        _context.SaveChanges();

        return Deleted;
    }

    private User? CreateOrUpdateUser(int? userId) =>
        userId is null ? null : _context.Users.Find(userId) ?? new User("NewUser");

    private IEnumerable<Tag> CreateOrUpdateTags(IEnumerable<string> tagNames)
    {
        var existing = _context.Tags.Where(p => tagNames.Contains(p.Name)).ToDictionary(p => p.Name);

        foreach (var tagName in tagNames)
        {
            existing.TryGetValue(tagName, out var tag);

            yield return tag ?? new Tag(tagName);
        }
    }
}