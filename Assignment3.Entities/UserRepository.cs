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
        //Bare test eksempel uden at have fulgt opgave kravene.
        var response = Response.Created;
        
        var newUser =  new User{
            Email = user.Email,
            Name = user.Name,
        };
        
        context.Users.Add(newUser);
        try
        {
        context.SaveChanges();
        } catch (Exception e)
        {
            response = Response.Conflict;
        }
        
        return (response, newUser.Id);
    }

    public IReadOnlyCollection<UserDTO> ReadAll()
    {
        throw new NotImplementedException();
    }

    public UserDTO Read(int userId)
    {
        throw new NotImplementedException();
    }

    public Response Update(UserUpdateDTO user)
    {
        try{
            var newUser = context.Users.Where(u => u.Id == user.Id).First();
            newUser.Email = user.Email;
            newUser.Name = user.Name;
            context.Users.Update(newUser);
            context.SaveChanges();
            return Response.Updated; 
        }
        catch{
            return Response.NotFound;
        }
    }

    public Response Delete(int userId, bool force = false)
    {
        if(!force) return Response.Conflict;
        try{
            var newUser = context.Users.Where(u => u.Id == userId).First();
            context.Users.Remove(newUser);
            context.SaveChanges();
            return Response.Deleted; 
        }
        catch{
            return Response.NotFound;
        }
    }
}