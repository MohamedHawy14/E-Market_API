using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FirstWebApiProject.Interfaces
{
    public interface IRepositry<T>where T :class
    {
        public void Add(T entity);

        public void Update(T entity);

        public void Delete(T entity);

        public T GetById(int Id);
        public T GetByName(string Name);

        public IEnumerable<T> GetALL();
        public IQueryable<T> GetAll();
        public T Find(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string includeProperties );

        public IQueryable<T> Include(Expression<Func<T, object>> navigationProperty);    }
}
