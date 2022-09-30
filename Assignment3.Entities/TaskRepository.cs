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
            else{
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

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        //Forkert, da den læser tags forkert.
        var all = new List<TaskDTO>(); 
        foreach (var task in context.Tasks)
        { 
            all.Add(new TaskDTO(Id:task.Id, Title:task.Title, task.UserID.ToString(), Tags: new []{String.Empty, },State:task.State));
        }

        return all;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        throw new NotImplementedException();
    }

    public TaskDetailsDTO Read(int taskId)
    {
        var task = context.Tasks.Find(taskId);
        if (task == null) return null;
        else
        { 
            return new TaskDetailsDTO(
                Id: task.Id,
                Title: task.Title,
                Description: task.Description,
                Created: DateTime.Now,
                AssignedToName: task.UserID.ToString(),
                Tags: new[] { String.Empty, },
                State: task.State,
                StateUpdated: task.StateUpdated
                );
        }
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