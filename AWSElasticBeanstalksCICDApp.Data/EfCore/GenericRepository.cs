using AWSElasticBeanstalksCICDApp.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWSElasticBeanstalksCICDApp.Data.EfCore
{
    public class GenericRepository<TEntity, TContext> : IRepository<TEntity> where TEntity:class 
        where TContext:AWSCICDAppDbContext,new()
    {
        public void Create(TEntity Entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Add(Entity);
                context.SaveChanges();
            }
        }

        public void Delete(TEntity Entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Remove(Entity);
                context.SaveChanges();
            }
        }

        public List<TEntity> GetAll()
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().ToList();
            }
        }

        public TEntity GetById(int id)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().Find(id);
            }
        }

        public void Update(TEntity Entity)
        {
            using (var context = new TContext())
            {
                context.Entry(Entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
