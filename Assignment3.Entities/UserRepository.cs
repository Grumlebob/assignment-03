using Assignment3.Core;

namespace Assignment3.Entities;

public class UserRepository : IUserRepository
{

    private readonly KanbanContext context;

    public UserRepository(KanbanContext context)
    {
        this.context = context;
    }

    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        var newUser = new User();
        newUser.Name = user.Name;
        var id = 0;
        try{
            id = context.Users.Where(x => x.Email == user.Email).First().Id;
        }catch{

        }
            if(context.Users.Find(id) != null) return (Response.Conflict, 00);
            else{
                newUser.Email = user.Email;
            }
            context.Users.Add(newUser);
            context.SaveChanges();
            return (Response.Created, newUser.Id);
        }
    

    public IReadOnlyCollection<UserDTO> ReadAll()
    {
        var all = new List<UserDTO>();
        foreach (var tag in context.Users)
        {
            all.Add(new UserDTO(tag.Id, tag.Name, tag.Email));
        }
        return all;
    }

    public UserDTO Read(int userId)
    {
        var u = context.Users.Find(userId);
        return new UserDTO(u.Id, u.Name, u.Email);
    }

    public Response Update(UserUpdateDTO user)
    {
        try
        {
            var newUser = context.Users.Where(u => u.Id == user.Id).First();
            newUser.Email = user.Email;
            newUser.Name = user.Name;
            context.Users.Update(newUser);
            context.SaveChanges();
            return Response.Updated;
        }
        catch
        {
            return Response.NotFound;
        }
    }

    public Response Delete(int userId, bool force = false)
    {
        if (context.Users.Find(userId)!.Tasks != null && !force) return Response.Conflict;
        else{
        try{
            var newUser = context.Users.Where(u => u.Id == userId).First();
            context.Users.Remove(newUser);
            context.SaveChanges();
            return Response.Deleted;
        }
        catch
        {
            return Response.NotFound;
        }
        }
    }
}