using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskTracker_Cosmos.API.DataAccess.Data;
using Task = TaskTracker_Cosmos.API.DataAccess.Entities.Task;
using TaskTracker_Cosmos.API.DataAccess.Entities;
using TaskTracker_Cosmos.API.DataAccess.Repository.Contracts;

namespace TaskTracker_Cosmos.API.DataAccess.Repository.Contracts
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        public TaskRepository(TaskTrackerCosmosDbContext context) : base(context)
        {
        }
        /*public void AddTask(int projectId, Task task)
        {
            var project = _context.Projects.Find(projectId);
            task.ProjectId = projectId;
            //project.Tasks.ToList().Add(task);
            _context.Tasks.Add(task);
            project.Tasks.Append(task).ToList();
        }

        public void RemoveTask(int projectId, int taskId)
        {
            var project = _context.Projects.Find(projectId);
            var task = _context.Tasks.Find(taskId);

            project.Tasks.ToList().Remove(task);
        }*/
    }
}
