using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Blog.Data.Abstractions;
using Blog.Model.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Blog.Data.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T>
            where T: class, IEntityBase, new()
    {
        private BlogContext _context;
        public EntityBaseRepository(BlogContext context)
        {
            _context = context;
        }
        public virtual void Add(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
           IQueryable<T> query = _context.Set<T>();
           foreach (var includeProperty in includeProperties)
           {
               query = query.Include(includeProperty);
           } 
           return query.AsEnumerable();
        }

        public virtual void Commit()
        {
            _context.SaveChanges();
        }

        public virtual int Count()
        {
           return _context.Set<T>().Count();
        }

        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);
            foreach (var entity in entities)
            {
               _context.Entry<T>(entity).State = EntityState.Deleted; 
            } 
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public virtual T GetSingle(string id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id); 
        }

        public virtual T GetSingle(Expression<Func<T,bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate); 
        }

        public virtual T GetSingle(Expression<Func<T,bool>> predicate,params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query =_context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
               query = query.Include(includeProperty); 
            }
            return query.FirstOrDefault(predicate);
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }
    }
}