using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DataAccess.Repository.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository TaskRepository { get; }
        IProjectRepository ProjectRepository { get; }

        void Save();
    }
}
