using System.Threading.Tasks;
using System.Collections.Generic;

namespace TestWebApplication.Data
{
    public interface IDataRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> SaveAsync(T entity);
    }
}
