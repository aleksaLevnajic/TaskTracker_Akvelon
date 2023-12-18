using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.DataAccess.Repository.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
