
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskTracker_Cosmos.API.DataAccess.Data;
using TaskTracker_Cosmos.API.DataAccess.Repository.Contracts;

namespace TaskTracker_Cosmos.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*using (var taskTrackerCosmosContext = new TaskTrackerCosmosDbContext())
            {
                /*var task1 = new TaskTracker_Cosmos.API.DataAccess.Entities.Task()
                {
                    Id = 1,
                    Name = "Task 1",
                    Description = "Task description 1",
                    Priority = 1,
                    Status = DataAccess.Entities.TaskStatus.ToDo,
                    ProjectId = 1
                };
                var task2 = new TaskTracker_Cosmos.API.DataAccess.Entities.Task()
                {
                    Id = 2,
                    Name = "Task 2",
                    Description = "Task description 2",
                    Priority = 1,
                    Status = DataAccess.Entities.TaskStatus.ToDo,
                    ProjectId = 1
                };
                var project1 = new TaskTracker_Cosmos.API.DataAccess.Entities.Project()
                {
                    Id = 1,
                    Name = "Project 1",
                    StartDate = new DateTime(2024, 3, 3),
                    EndDate = new DateTime(2024, 4, 4),
                    Priority = 1,
                    Status = DataAccess.Entities.ProjectStatus.NotStarted
                };
                var project2 = new TaskTracker_Cosmos.API.DataAccess.Entities.Project()
                {
                    Id = 2,
                    Name = "Project 2",
                    StartDate = new DateTime(2024, 3, 3),
                    EndDate = new DateTime(2024, 4, 4),
                    Priority = 1,
                    Status = DataAccess.Entities.ProjectStatus.NotStarted,
                    Tasks = new List<TaskTracker_Cosmos.API.DataAccess.Entities.Task>
                    { 
                        new TaskTracker_Cosmos.API.DataAccess.Entities.Task()
                        {
                            Id = 3,
                            Name = "Task 3",
                            Description = "Task description 3",
                            Priority = 1,
                            Status = DataAccess.Entities.TaskStatus.ToDo,
                            ProjectId = 2
                        }
                    }
                };

                /*taskTrackerCosmosContext.Tasks.Add(task1);
                taskTrackerCosmosContext.Tasks.Add(task2);
                taskTrackerCosmosContext.Projects.Add(project1);
                taskTrackerCosmosContext.Projects.Add(project2);

                taskTrackerCosmosContext.SaveChanges();
            }*/

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<TaskTrackerCosmosDbContext>(opt =>
            {
                opt.UseCosmos(builder.Configuration.GetSection("CosmosDb:AccountEndpointUri").Value!,
                              builder.Configuration.GetSection("CosmosDb:PrimaryKey").Value!,
                              builder.Configuration.GetSection("CosmosDb:DatabaseName").Value!);
            });

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

            builder.Services.AddControllers().AddJsonOptions(opt =>
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
