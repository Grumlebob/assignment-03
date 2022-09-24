using Assignment3.Core;
using Assignment3.Entities;
using Microsoft.EntityFrameworkCore;
using Task = Assignment3.Entities.Task;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
var connectionString = configuration.GetConnectionString("ConnectionString");
var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
optionsBuilder.UseNpgsql(connectionString);
var options = optionsBuilder.Options;
using var context = new KanbanContext(options);


var tagRepository = new TagRepository(context);
var taskRepository = new TaskRepository(context);
var userRepository = new UserRepository(context);


Console.Write("Enter Name: ");
var inputName = Console.ReadLine();
Console.Write("Enter Email: ");
var inputEmail = Console.ReadLine();

UserCreateDTO userCreateDTO = new UserCreateDTO(Email: inputEmail, Name:inputName);

var (response, user) = userRepository.Create(userCreateDTO);


/*

var users = context.Users
    .Where(u => u.Name.Contains(input))
    .ToList();

foreach (var user in users)
    Console.WriteLine(user);


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