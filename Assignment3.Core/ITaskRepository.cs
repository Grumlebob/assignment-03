namespace Assignment3.Core;

public interface ITaskRepository
{
    (Response Response, int TaskId) Create(TaskCreateDTO workItem);
    TaskDetailsDTO Find(int workItemId);
    IReadOnlyCollection<TaskDTO> Read();
    IReadOnlyCollection<TaskDTO> ReadRemoved();
    IReadOnlyCollection<TaskDTO> ReadByTag(string tag);
    IReadOnlyCollection<TaskDTO> ReadByUser(int userId);
    IReadOnlyCollection<TaskDTO> ReadByState(State state);
    Response Update(TaskUpdateDTO workItem);
    Response Delete(int workItemId);
}
