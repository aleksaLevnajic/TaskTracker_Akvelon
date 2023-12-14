using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskTracker.DataAccess.Entities;
using Task = TaskTracker.DataAccess.Entities.Task;

namespace TaskTracker.DataAccess.Repository.Contracts
{
    public interface IProjectRepository : IRepository<Project>
    {       
        IEnumerable<Task> ViewAllTasks(int id);
    }
}
