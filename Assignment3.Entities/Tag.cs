using System.ComponentModel.DataAnnotations;

namespace Assignment3.Entities;

public class Tag
{
    public Tag()
    {
        Tags = new List<Tag>();
    }
    //Id : int
    public int Id { get; set; }
    //Name : string(50), required, unique
    [Required, MaxLength(50)]
    public string Name { get; set; }
    //Tasks : many-to-many reference to Task entity
    public ICollection<Tag> Tags { get; set; }

}
