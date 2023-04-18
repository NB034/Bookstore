
namespace Bookstore.DataAccess.Repositories
{
    internal interface IRepository<T>
    {
        void CreateEntity(T entity);
        T GetEntity(int id);
        void UpdateEntity(T entity);
        void DeleteEntity(int id);
        void Save();
    }
}
