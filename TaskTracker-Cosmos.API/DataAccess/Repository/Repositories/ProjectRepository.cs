using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker_Cosmos.API.DataAccess.Data;
using TaskTracker_Cosmos.API.DataAccess.Entities;
using TaskTracker_Cosmos.API.DataAccess.Repository.Contracts;

namespace TaskTracker_Cosmos.API.DataAccess.Repository.Contracts
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(TaskTrackerCosmosDbContext context) : base(context)
        {
        }        

        /*public IEnumerable<Entities.Task> ViewAllTasks(int id)
        {
            var tasks = _context.Projects.Where(x => x.Id == id).SelectMany(p => p.Tasks);

            return tasks;
        }*/
    }
}
