using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MiniFB.DAL.Repositories
{
    public interface IRepository<TEntity> 
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string includeproperties = null);
        TEntity GetById(object id);
        void Create(TEntity entity);
        void Delete(object id);
        void Update(TEntity entity);
    }
}
