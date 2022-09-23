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

Console.Write("Enter query to search for: ");

var input = Console.ReadLine();

using var context = new KanbanContext(options);

var SomeNewUser =  new User{
    Email = "EnEmail",
    Name = "Hansd",
    Tasks = new List<Task>()
};


//context.Add(SomeNewUser);
//context.Remove(SomeNewUser);
//context.SaveChanges();

var users = context.Users
    .Where(u => u.Name.Contains(input))
    .ToList();

foreach (var user in users)
    Console.WriteLine(user);

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