
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Repository.Contracts;
using TaskTracker.DataAccess.Repository.Repositories;

namespace TaskTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<TaskTrackerDbContext>(options =>
                options.UseSqlServer(
                builder.Configuration.GetConnectionString("SqlServerCS")));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddControllers().AddJsonOptions(opt =>
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);


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
