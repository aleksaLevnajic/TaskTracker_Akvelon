using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskTracker_Cosmos.API.DataAccess.Data;
using TaskTracker_Cosmos.API.DataAccess.Entities;
using TaskTracker_Cosmos.API.DataAccess.Repository.Contracts;

namespace TaskTracker_Cosmos.API.DataAccess.Repository.Contracts
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public readonly TaskTrackerCosmosDbContext _context;

        public Repository(TaskTrackerCosmosDbContext context)
        {
            _context = context;
        }

        public T Get(int id, params Expression<Func<T, object>>[] includes)
        {
            var result = _context.Set<T>().AsQueryable();


            return result.FirstOrDefault(x => x.Id == id);            
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var result = _context.Set<T>().AsQueryable();

      
            return result.ToList();
        }
        public T GetExp(Expression<Func<T, bool>>? expression)
        {
            IQueryable<T> query = _context.Set<T>().Where(expression);

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAllExp(Expression<Func<T, bool>>? expression)
        {
            IQueryable<T> query = _context.Set<T>().Where(expression);

            return query.ToList();
        }
    }
}
