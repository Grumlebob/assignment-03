using System.ComponentModel.DataAnnotations;

namespace Assignment3.Entities;

public class User
/*
Id : int
Name : string(100), required
Email : string(100), required, unique
Tasks : list of Task entities belonging to User
 */
{
    public User()
    {
        Tasks = new List<Task>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public virtual List<Task> Tasks { get; set; }
}