using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskTracker.DataAccess.Entities;
using Task = TaskTracker.DataAccess.Entities.Task;

namespace TaskTracker.DataAccess.Repository.Contracts
{
    public interface ITaskRepository : IRepository<Task>
    {
        void AddTask(int projectId, Task task);
        //void RemoveTask(int projectId, Task task);
        void RemoveTask(int projectId, int taskId);
    }
}
