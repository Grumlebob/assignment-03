using Assignment3.Entities;
using Microsoft.EntityFrameworkCore;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
var connectionString = configuration.GetConnectionString("ConnectionString");

var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();

optionsBuilder.UseNpgsql(connectionString);

var options = optionsBuilder.Options;

Console.Write("Enter query to search for: ");
