using Assignment3.Core;
using Assignment3.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Task = Assignment3.Entities.Task;

var factory = new KanbanContextFactory();
using var context = factory.CreateDbContext(args);


var tagRepository = new TagRepository(context);
var taskRepository = new TaskRepository(context);
var userRepository = new UserRepository(context);

Console.Write(@"Hello! - Enter number for repository you want to interact with:
Tags: 1 
Tasks: 2 
Users: 3 
Select: ");
int navigation = Convert.ToInt32(Console.ReadLine());

if (navigation == 1) //Tags
{
    Console.Write(@"Tag options - Enter number:
Create: 1 
Read: 2 
Delete: 3 
Select: ");
    navigation = Convert.ToInt32(Console.ReadLine());
    if (navigation == 1) //Create
    {
        //CREATE TAG EXAMPLE:
        Console.WriteLine("Create a Tag!");
        Console.Write("Enter Name: ");
        var inputName = Console.ReadLine();
        
        TagCreateDTO createTag = new TagCreateDTO(Name: inputName);
        var (response, userId) = tagRepository.Create(createTag);

        Console.WriteLine($"You attempted to create: {inputName} id: {userId} \nWith response: {response}");
    }
    else if (navigation == 2) //Read
    {
    
    }
    else if (navigation == 3) //Delete
    {
        
    }
}
else if (navigation == 2) //Task
{
    Console.Write(@"Task options - Enter number:
Create: 1 
Read: 2 
Delete: 3 
Select: ");
    navigation = Convert.ToInt32(Console.ReadLine());
    if (navigation == 1) //Create
    {
       
    }
    else if (navigation == 2) //Read
    {
    
    }
    else if (navigation == 3) //Delete
    {
        
    }
}
else if (navigation == 3) //Users
{
    Console.Write(@"User options - Enter number:
Create: 1 
Read: 2 
Delete: 3 
Select: ");
    navigation = Convert.ToInt32(Console.ReadLine());
    if (navigation == 1) //Create
    {
        //CREATE USER EXAMPLE:
        Console.WriteLine("Create an user!");
        Console.Write("Enter Name: ");
        var inputName = Console.ReadLine();
        Console.Write("Enter Email: ");
        var inputEmail = Console.ReadLine();

        UserCreateDTO userCreateDTO = new UserCreateDTO(Email: inputEmail, Name: inputName);
        var (response, userId) = userRepository.Create(userCreateDTO);

        Console.WriteLine("You attempted to create: " + inputEmail + " " + inputName + " id: " + userId + " \n" +
                          "With response: " + response);
    }
    else if (navigation == 2) //Read
    {
    
    }
    else if (navigation == 3) //Delete
    {
        
    }
}
else
{
    throw new Exception(("Invalid input"));
}


/*

var query = context.Tasks
    .Where(t => t.Title.Contains(input) || t.Description.Contains(input))
    .Select(t => new
    {
        t.Title,
        t.Description,
        t.State,
        t.Tags
    });

var query2 = from c in context.Tags
    join d in context.Tasks on c.Id equals d.Id
    where c.Name.Contains(input)
            select new
            {
                c.Name,
                c.Tags
            };

foreach (var task in query)
    Console.WriteLine($"{task.Title} - {task.Description} - {task.State} - {task.Tags}");

foreach (var task in query2)
    Console.WriteLine($"{task.Name} - {task.Tags}");
*/