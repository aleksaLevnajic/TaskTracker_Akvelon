using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Repository.Contracts;

namespace TaskTracker.DataAccess.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskTrackerDbContext _context;

        public UnitOfWork(TaskTrackerDbContext context)
        {
            _context = context;
            TaskRepository = new TaskRepository(_context);
            ProjectRepository = new ProjectRepository(_context);
            UserRepository = new UserRepository(_context);
        }

        public ITaskRepository TaskRepository { get; private set; }
        public IProjectRepository ProjectRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
