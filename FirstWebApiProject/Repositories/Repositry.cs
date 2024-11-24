using FirstWebApiProject.Interfaces;
using FirstWebApiProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FirstWebApiProject.Repositories
{
    public class Repositry<T> : IRepositry<T> where T : class
    {
        private readonly MarketContext context;

        public Repositry(MarketContext context)
        {
            this.context = context;
        }
        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetALL() => context.Set<T>().AsNoTracking().ToList();

        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

        public T Find(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            return query.FirstOrDefault(predicate);
        }




        public T GetById(int Id) => context.Set<T>().Find(Id);


        // public T GetByName(string Name)=> context.Set<T>().Find(Name);

        public T GetByName(string name)=> context.Set<T>().FirstOrDefault(e => EF.Property<string>(e, "Name") == name);



        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string includeProperties = null)
        {
            // Start with the DbSet of the current entity type
            IQueryable<T> query = context.Set<T>();

            // Apply the filter if it's provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include any related properties if specified
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            // Return the first matching entity or null if none exist
            return query.FirstOrDefault();
        }

        public IQueryable<T> Include(Expression<Func<T, object>> navigationProperty)
        {
            IQueryable<T> query = context.Set<T>();

            // Apply the include if a valid navigation property is specified
            if (navigationProperty != null)
            {
                query = query.Include(navigationProperty);
            }

            return query;
        }



    }
}
