using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Entities;
using TaskTracker.DataAccess.Repository.Contracts;

namespace TaskTracker.DataAccess.Repository.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(TaskTrackerDbContext context) : base(context)
        {
        }        

        public IEnumerable<Entities.Task> ViewAllTasks(int id)
        {
            var tasks = _context.Projects.Where(x => x.Id == id).SelectMany(p => p.Tasks);

            return tasks;
        }
    }
}
