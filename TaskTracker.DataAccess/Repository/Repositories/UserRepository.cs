using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Repository.Contracts;
using TaskTracker.Domain.Entities;

namespace TaskTracker.DataAccess.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TaskTrackerDbContext context) : base(context)
        {
        }
    }
}
