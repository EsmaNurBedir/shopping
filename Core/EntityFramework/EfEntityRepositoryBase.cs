using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
         where TEntity : class, IEntity, new()
         where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using (var TContext = new TContext())
            {
                var added = TContext.Entry(entity);
                added.State = EntityState.Added;
                TContext.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (var TContext = new TContext())
            {
                var deleted = TContext.Entry(entity);
                deleted.State = EntityState.Deleted;
                TContext.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var Context = new TContext())
            {
                return filter == null ?
                    Context.Set<TEntity>().ToList()
                    : Context.Set<TEntity>().Where(filter).ToList();

            }
        }

        public void Update(TEntity entity)
        {
            using (var TContext = new TContext())
            {
                var updated = TContext.Entry(entity);
                updated.State = EntityState.Modified;
                TContext.SaveChanges();
            }
        }
    }
}
