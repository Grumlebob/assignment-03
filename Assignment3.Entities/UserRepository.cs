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
        context.SaveChanges();
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
        throw new NotImplementedException();
    }

    public Response Delete(int userId, bool force = false)
    {
        throw new NotImplementedException();
    }
}