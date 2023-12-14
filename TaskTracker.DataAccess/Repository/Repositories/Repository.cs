using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Entities;
using TaskTracker.DataAccess.Repository.Contracts;

namespace TaskTracker.DataAccess.Repository.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public readonly TaskTrackerDbContext _context;

        public Repository(TaskTrackerDbContext context)
        {
            _context = context;
            //_context.Projects.Include(x => x.Tasks);
            //_context.Tasks.Include(x => x.Project).Include(x => x.ProjectId);
        }

        public T Get(int id, params Expression<Func<T, object>>[] includes)
        {
            var result = _context.Set<T>().AsQueryable();

            foreach( var include in includes)
            {
                result = result.Include(include);
            }

            return result.FirstOrDefault(x => x.Id == id);            
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var result = _context.Set<T>().AsQueryable();

            foreach(var include in includes)
            {
                result = result.Include(include);
            }    

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

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }        
    }
}
