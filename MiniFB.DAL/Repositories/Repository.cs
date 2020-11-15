using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MiniFB.DAL.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity: class
    {
        private readonly IMiniFbContext context;
        protected readonly DbSet<TEntity> dbSet;
        public Repository(IMiniFbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string includeproperties = null)
        {
            IQueryable<TEntity> query = dbSet;
            if(includeproperties != null)
            {
                var props = includeproperties.Split(",");
                foreach(var prop in props)
                {
                    query.Include(prop);
                }
            }
            if(filter != null)
            {
                query.Where(filter);
            }

            return query.AsEnumerable(); 
        }

        public TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public void Create(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(object id)
        {
            TEntity entity = dbSet.Find(id);
            dbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

       
    }
}
