using FirstWebApiProject.Repositories;

namespace FirstWebApiProject.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        Repositry<T> Repositry<T>() where T : class;

        int Complete();
    }
}
