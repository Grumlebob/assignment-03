using System.Net;
using Assignment3.Core;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{

    private readonly KanbanContext context;

    public TaskRepository(KanbanContext context)
    {
        this.context = context;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var response = Response.Created;
        var newtask = new Task();
        try
        {
            if (context.Users.Find(task.AssignedToId) == null) return (Response.BadRequest, 0);
            else
            {
                newtask.Description = task.Description;
                newtask.UserID = task.AssignedToId;
                newtask.Title = task.Title;
                newtask.State = State.New;
                newtask.StateUpdated = DateTime.Now;
                context.Tasks.Add(newtask);
                context.SaveChanges();
            }
        }
        catch
        {
            response = Response.Conflict;
        }

        return (response, newtask.Id);

    }

    public TaskDetailsDTO Find(int taskId)
    {
        foreach (var task in context.Tasks)
        {
            var tags = new List<String>();
            foreach (var tag in task.Tags) tags.Add(tag.ToString()!);
            if (task.Id == taskId)
            {
                return new TaskDetailsDTO(Id: task.Id, Title: task.Title!, Description: task.Description!, Created: task.StateUpdated, AssignedToName: (context.Users.Find(task.UserID)!.Name)!, Tags: tags, State: task.State, task.StateUpdated);
            }
        }return null!;
    }
    

    public IReadOnlyCollection<TaskDTO> Read()
    {
        var all = new List<TaskDTO>();

        foreach (var task in context.Tasks)
        {
            var tags = new List<String>();
            foreach (var tag in task.Tags) tags.Add(tag.ToString()!);

            all.Add(new TaskDTO(Id: task.Id, Title: task.Title!, AssignedToName: (context.Users.Find(task.UserID)!.Name!), Tags: tags, State: task.State));
        }
        if (all == null) return null!;
        else return all;
    }

    public IReadOnlyCollection<TaskDTO> ReadRemoved()
    {
        var all = new List<TaskDTO>();
        foreach (var task in context.Tasks)
        {
            var tags = new List<String>();
            foreach (var tag in task.Tags) tags.Add(tag.ToString()!);

            if (task.State == State.Removed) all.Add(new TaskDTO(Id: task.Id, Title: task.Title!, AssignedToName: (context.Users.Find(task.UserID)!.Name!), Tags: tags, State: task.State));
        }
        if (all == null) return null!;
        else return all;
    }

    public IReadOnlyCollection<TaskDTO> ReadByTag(string tag)
    {
        var all = new List<TaskDTO>();
        foreach (var task in context.Tasks)
        {
            var tags = new List<String>();
            foreach (var t in task.Tags)
            {
                tags.Add(tag.ToString());
                if (t.ToString() == tag) all.Add(new TaskDTO(Id: task.Id, Title: task.Title!, AssignedToName: (context.Users.Find(task.UserID)!.Name!), Tags: tags, State: task.State));
            }
        }
        if (all == null) return null!;
        else return all;
    }

    public IReadOnlyCollection<TaskDTO> ReadByUser(int userId)
    {
        var all = new List<TaskDTO>();
        foreach (var task in context.Tasks)
        {
            var tags = new List<String>();
            foreach (var tag in task.Tags) tags.Add(tag.ToString()!);
            if (task.UserID == userId) all.Add(new TaskDTO(Id: task.Id, Title: task.Title!, AssignedToName: (context.Users.Find(task.UserID)!.Name!), Tags: tags, State: task.State));

        }
        if (all == null) return null!;
        else return all;
    }

    public IReadOnlyCollection<TaskDTO> ReadByState(State state)
    {
        var all = new List<TaskDTO>();
        foreach (var task in context.Tasks)
        {
            var tags = new List<String>();
            foreach (var tag in task.Tags) tags.Add(tag.ToString()!);
            if (task.State == state) all.Add(new TaskDTO(Id: task.Id, Title: task.Title!, AssignedToName: (context.Users.Find(task.UserID)!.Name!), Tags: tags, State: task.State));
        }
        if (all == null) return null!;
        else return all;
    }
    public Response Update(TaskUpdateDTO task)
        {
            try
            {
                var newTask = context.Tasks.Where(t => t.Id == task.Id).First();
                if (newTask.State != task.State)
                {
                    newTask.State = task.State;
                    newTask.StateUpdated = DateTime.Now;
                }
                if (context.Users.Find(task.AssignedToId) == null) return Response.BadRequest;
                else
                {
                    if (task.Tags != null) newTask.Tags = (List<Tag>)task.Tags;

                    newTask.Description = task.Description;
                    newTask.Title = task.Title;
                    newTask.UserID = task.AssignedToId;
                    context.Tasks.Update(newTask);
                    context.SaveChanges();
                    return Response.Updated;
                }
            }
            catch
            {
                return Response.NotFound;
            }
        }

        public Response Delete(int taskId)
        {
            try
            {
                var newTask = context.Tasks.Where(t => t.Id == taskId).First();
                if (newTask!.State == State.New)
                {
                    context.Remove(newTask!);
                    context.SaveChanges();
                    return Response.Deleted;
                }
                else if (newTask!.State == State.Active)
                {
                    newTask.State = State.Removed;
                    return Response.Conflict;
                }
                else return Response.Conflict;

            }
            catch
            {
                return Response.NotFound;
            }
        }
    }