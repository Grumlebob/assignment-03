using System.ComponentModel.DataAnnotations;
namespace Assignment3.Entities;

public class Task
{

    public Task()
    {
        Tags = new List<Tag>();
    }

    //Id : int
    public int Id { get; set; }
    
    //Title : string(100), required
    [Required, MaxLength(100)]
    public string Title { get; set; }
    
    //AssignedTo : optional reference to User entity
    public User? AssignedTo { get; set; }
    
    //Description : string(max), optional
    [MaxLength(Int32.MaxValue)]
    public string? Description { get; set; }
    
    //Tags : many-to-many reference to Tag entity
    public ICollection<Tag> Tags { get; set; }
    
    public StateType State { get; set; }
    
    //State : enum (New, Active, Resolved, Closed, Removed), required
    public enum StateType
    {
        New,
        Active,
        Resolved,
        Closed,
        Removed
    }
    
}
