using Assignment3.Core;
using static Assignment3.Core.Response;

namespace Assignment3.Entities;

public class UserRepository : IUserRepository
{

    private readonly KanbanContext _context;

    public UserRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        var entity = _context.Users.FirstOrDefault(c => c.Name == user.Name);
        Response response;

        if (entity is null)
        {
            entity = new User(user.Name);

            _context.Users.Add(entity);
            _context.SaveChanges();

            response = Created;
        }
        else
        {
            response = Conflict;
        }

        var created = new UserDTO(entity.Id, entity.Name, entity.Email);

        return (response, created.Id);
    }

    public UserDTO Find(int userId)
    {
        var user = from c in _context.Users
            where c.Id == userId
            select new UserDTO(c.Id, c.Name,c.Email);

        return user.FirstOrDefault()!;
    }
    


    public IReadOnlyCollection<UserDTO> Read()
    {
        var cities = from c in _context.Users
            orderby c.Id
            select new UserDTO(c.Id, c.Name,c.Email);

        return cities.ToArray();
    }

    public Response Update(UserUpdateDTO user)
    {
        var entity = _context.Users.Find(user.Id);
        Response status;

        if (entity is null)
        {
            status = NotFound;
        }
        else
        {
            entity.Name = user.Name;
            _context.SaveChanges();
            status = Updated;
        }

        return status;
    }

    public Response Delete(int userId, bool force = false)
    {
        var user = _context.Users.FirstOrDefault(c => c.Id == userId);
        Response status;

        if (user is null)
        {
            status = NotFound;
        }
        else if (!force)
        {
            status = Conflict;
        }

        else if (user.Tasks.Any())
        {
            status = Conflict;
        }
        else
        {
            _context.Users.Remove(user);
            _context.SaveChanges();

            status = Deleted;
        }

        return status;
    }
}