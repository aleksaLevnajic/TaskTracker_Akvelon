using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskTracker.DataAccess.Entities;
using Task = TaskTracker.DataAccess.Entities.Task; // it has to have whole namespace bc Task class interferes with System.Threading.Task class

namespace TaskTracker.DataAccess.Data
{
    public class TaskTrackerDbContext : DbContext
    {
        public TaskTrackerDbContext()
        { 
            
        }

        public TaskTrackerDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Task> Tasks { get; set; } 
        public virtual DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Setting properties, also could have been done using DataAnnotations
            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

                entity.HasMany(p => p.Tasks).WithOne(t => t.Project).HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Cascade);
            });

            //Seeding inital data to work with
            modelBuilder.Entity<Task>().HasData(
                new Task { Id = 1, Name = "Initial meeting", Description = "Present project.", Priority = 1, Status = Entities.TaskStatus.Done, ProjectId = 1 },
                new Task { Id = 2, Name = "Develop App", Description = "Building software soultion for Task Tracker app.", Priority = 2, Status = Entities.TaskStatus.InProgress, ProjectId = 1 },
                new Task { Id = 3, Name = "Final meeting", Description = "Final pre-production meeting.", Priority = 3, Status = Entities.TaskStatus.ToDo, ProjectId = 1 }
                );

            modelBuilder.Entity<Project>().HasData(
                new Project { Id = 1, Name = "Task Tracker App", StartDate = DateTime.UtcNow, EndDate = null, Priority = 1, Status = ProjectStatus.Active }
                );            
        }
    }


}
