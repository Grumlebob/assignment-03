using System.ComponentModel.DataAnnotations;

namespace Assignment3.Entities;

public class Tag
{
    public Tag(string name)
    {
        Name = name;
        Tasks = new List<Task>();
    }
    //Id : int
    public int Id { get; set; }
    //Name : string(50), required, unique
    public string Name { get; set; } = null!;
    //Tasks : many-to-many reference to Task entity
    public virtual List<Task> Tasks { get; set; }

}
