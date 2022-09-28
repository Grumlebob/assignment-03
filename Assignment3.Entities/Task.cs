using System.ComponentModel.DataAnnotations;
using Assignment3.Core;

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
    public string Title { get; set; }
    
    //Description : string(max), optional
    public string? Description { get; set; }
    
    //AssignedTo - renamed User: optional reference to User entity
    public User? User { get; set; }
    //Following convention 4 https://www.entityframeworktutorial.net/efcore/one-to-many-conventions-entity-framework-core.aspx
    public int? UserID { get; set; }

    //Tags : many-to-many reference to Tag entity
    public virtual List<Tag> Tags { get; set; }
    
    public State State { get; set; }
    
    public DateTime StateUpdated { get; set;}
    //State : enum (New, Active, Resolved, Closed, Removed), required
    
    
}
