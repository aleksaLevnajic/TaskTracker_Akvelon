using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Entities;
using TaskTracker.Domain.Entities;
using Task = TaskTracker.DataAccess.Entities.Task;

namespace TaskTrackerAPi.IntefrationTests.DbContext
{
    public class MockTaskTrackerDbContext : TaskTrackerDbContext
    {
        public MockTaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options)
            :base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var startDate = new DateTime(2024, 11, 11);
            var endDate = new DateTime(2024, 12, 12);

            modelBuilder.Entity<Project>().HasData(
                new Project { Id = 100, Name = "Test1", StartDate = startDate, EndDate = endDate, Priority = 1, Status = ProjectStatus.NotStarted },
                new Project { Id = 101, Name = "Test2", StartDate = startDate, EndDate = endDate, Priority = 1, Status = ProjectStatus.NotStarted }
            );
            modelBuilder.Entity<Task>().HasData(
               new Task
               {
                   Id = 98,
                   Name = "TestTask1",
                   Description = "Task description test 1",
                   Priority = 1,
                   Status = TaskTracker.DataAccess.Entities.TaskStatus.ToDo
               },
                new Task
                {
                    Id = 99,
                    Name = "TestTask2",
                    Description = "Task description test 2",
                    Priority = 1,
                    Status = TaskTracker.DataAccess.Entities.TaskStatus.ToDo
                }
            );
        }
    }
}
