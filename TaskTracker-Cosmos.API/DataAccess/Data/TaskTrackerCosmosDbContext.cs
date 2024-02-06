using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TaskTracker_Cosmos.API.DataAccess.Entities;
using Task = TaskTracker_Cosmos.API.DataAccess.Entities.Task;

namespace TaskTracker_Cosmos.API.DataAccess.Data
{
    public class TaskTrackerCosmosDbContext : DbContext
    {
        public TaskTrackerCosmosDbContext()
        {
            
        }
        public TaskTrackerCosmosDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Project> Projects { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().Property(p => p.Id).HasConversion(id => id.ToString(), id => int.Parse(id));
            modelBuilder.Entity<Project>().Property(p => p.Id).HasConversion(id => id.ToString(), id => int.Parse(id));  
            
            
            /*int result;
            modelBuilder.Entity<Task>().Property(p => p.Id).HasConversion(id => id.ToString(), id => int.TryParse(id, out result) ? result : 0);
            modelBuilder.Entity<Project>().Property(p => p.Id).HasConversion(id => id.ToString(), id => int.TryParse(id, out result) ? result : 0);*/



            modelBuilder.Entity<Task>().ToContainer("Tasks").HasPartitionKey(k => k.Id);

            modelBuilder.Entity<Project>().ToContainer("Projects").HasPartitionKey(k => k.Id);

            modelBuilder.Entity<Project>().HasMany(t => t.Tasks).WithOne(p => p.Project).HasForeignKey(k => k.ProjectId);
        }
    }
}
