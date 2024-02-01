using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_Cosmos.API.DataAccess.Repository.Contracts
{
    public interface IRepository<T> where T : class
    {
        T Get(int id, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
        T GetExp(Expression<Func<T, bool>>? expression);
        IEnumerable<T> GetAllExp(Expression<Func<T, bool>>? expression);
    }
}
