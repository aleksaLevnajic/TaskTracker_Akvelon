﻿using Microsoft.EntityFrameworkCore;
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

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos("https://task-tracker-cdb.documents.azure.com:443/",
                "iGfMUk8DqEz6BcTc5x2uT190bnRrOIdzsrXoximJItqS41sZ5JAJ1V828dEOx6sA8l7U1w0AonwkACDbMdoTiA==",
                "tasktracker-db");
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().Property(p => p.Id).HasConversion(id => id.ToString(), id => int.Parse(id));
            modelBuilder.Entity<Project>().Property(p => p.Id).HasConversion(id => id.ToString(), id => int.Parse(id));

            modelBuilder.Entity<Task>().ToContainer("Tasks").HasPartitionKey(k => k.Id);

            modelBuilder.Entity<Project>().ToContainer("Projects").HasPartitionKey(k => k.Id);

            modelBuilder.Entity<Project>().HasMany(t => t.Tasks).WithOne(p => p.Project).HasForeignKey(k => k.ProjectId);
        }
    }
}