using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Repositories.Interfaces
{
    interface ICustomerRepository<T> where T : IdentityUser<Guid>
    {
        Task<IdentityResult> Create(T entity, string password);
        Task Update(T entity);
        Task Delete(Guid identifier);
        Task<T> FindById(Guid identifier);
        Task<T> FindByEmail(string email);
        Task<List<T>> GetAll();
    }
}
