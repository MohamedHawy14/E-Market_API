using FirstWebApiProject.Interfaces;
using FirstWebApiProject.Models;
using System.Collections;

namespace FirstWebApiProject.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MarketContext dbcontext;

        private Hashtable _repsitories;
        public UnitOfWork(MarketContext dbcontext)
        {
            this.dbcontext = dbcontext;
            _repsitories = new Hashtable();


        }
        public Repositry<T> Repositry<T>() where T : class
        {
            var Key = typeof(T).Name;
            if (!_repsitories.ContainsKey(Key))
            {
                var repo = new Repositry<T>(dbcontext);
                _repsitories.Add(Key, repo);
            }
            return _repsitories[Key] as Repositry<T>;
        }


        public int Complete()
        {
            return dbcontext.SaveChanges();

        }

        public void Dispose()
        {
            dbcontext.Dispose();
        }

     
    }
}
