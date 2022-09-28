﻿using Assignment3.Core;

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
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public Response Update(TaskUpdateDTO task)
    {
        try
        {
            var newTask = context.Tasks.Where(t => t.Id == task.Id).First();
            newTask.Description = task.Description;
            newTask.Title = task.Title;
            newTask.UserID = task.AssignedToId;
            context.Tasks.Update(newTask);
            context.SaveChanges();
            return Response.Updated;
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
            var newTask = context.Tasks.Where(t => t.Id == taskId).FirstOrDefault();
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