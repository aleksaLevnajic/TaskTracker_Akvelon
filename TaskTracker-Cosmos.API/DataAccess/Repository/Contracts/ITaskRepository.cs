using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskTracker_Cosmos.API.DataAccess.Entities;
using Task = TaskTracker_Cosmos.API.DataAccess.Entities.Task;

namespace TaskTracker_Cosmos.API.DataAccess.Repository.Contracts
{
    public interface ITaskRepository : IRepository<Task>
    {
        //void AddTask(int projectId, Task task);
        //void RemoveTask(int projectId, Task task);
        //void RemoveTask(int projectId, int taskId);
    }
}
