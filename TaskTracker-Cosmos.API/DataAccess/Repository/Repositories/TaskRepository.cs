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
        
    }
}
