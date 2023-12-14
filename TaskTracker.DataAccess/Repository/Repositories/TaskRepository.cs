using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Entities;
using Task = TaskTracker.DataAccess.Entities.Task;
using TaskTracker.DataAccess.Repository.Contracts;

namespace TaskTracker.DataAccess.Repository.Repositories
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        public TaskRepository(TaskTrackerDbContext context) : base(context)
        {
        }
        public void AddTask(int projectId, Task task)
        {
            var project = _context.Projects.Find(projectId);
            task.ProjectId = projectId;
            project.Tasks.ToList().Add(task);
        }

        public void RemoveTask(int projectId, int taskId)
        {
            var project = _context.Projects.Find(projectId);
            var task = _context.Tasks.Find(taskId);

            project.Tasks.ToList().Remove(task);
        }

        /*public void RemoveTask(int projectId, Task task)
        {
            var project = _context.Projects.Find(projectId);
            project.Tasks.ToList().Remove(task);
        }*/
    }
}
