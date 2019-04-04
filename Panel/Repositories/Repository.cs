using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Panel.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        public Repository(DbContext context)
        {
            Context = context;
        }

        public void Add(TEntity entity) => Context.Set<TEntity>().Add(entity);

        public void AddRange(IEnumerable<TEntity> entities) => 
            Context.Set<TEntity>().AddRange(entities);

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => 
            Context.Set<TEntity>().Where(predicate);

        public TEntity Get(int id) => Context.Set<TEntity>().Find(id);

        public IEnumerable<TEntity> GetAll() => Context.Set<TEntity>().ToList();

        public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities) => 
            Context.Set<TEntity>().RemoveRange(entities);

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var x in entities)
            {
                DbSet<TEntity> Dbset = Context.Set<TEntity>();
                object xSNValue = x.GetType().GetProperty("SN").GetValue(x);

                foreach (var y in Dbset)
                {
                    object ySNValue = y.GetType().GetProperty("SN").GetValue(y);
                    if ((int)xSNValue == (int)ySNValue)
                    {
                        Context.Set<TEntity>().Remove(y);
                        break;
                    }
                }
            }

            int j = 1;

            IEnumerable<DbEntityEntry<TEntity>> unchangeditems = Context.ChangeTracker.Entries<TEntity>()
                .Where(e => e.State == EntityState.Unchanged);

            foreach (var unitem in unchangeditems)
            {
                unitem.Entity.GetType().GetProperty("SN").SetValue(unitem.Entity, j);
                unitem.State = EntityState.Modified;
                j++;
            }
        }

        public void DeleteAll()
        {
            foreach (var item in Context.Set<TEntity>())
            {
                Context.Set<TEntity>().Remove(item);
            }
        }

    }
}
