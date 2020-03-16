using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);
        void Update(T entity);
        Task Delete(Guid identifier);
        Task<T> Get(Guid identifier);
        Task<List<T>> GetAll();
    }
}
