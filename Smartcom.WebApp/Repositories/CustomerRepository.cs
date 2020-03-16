using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smartcom.WebApp.Database;
using Smartcom.WebApp.Models;
using Smartcom.WebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Repositories
{
    public class CustomerRepository : ICustomerRepository<Customer>
    {
        private readonly AppDataBaseContext dbContext;
        private readonly UserManager<Customer> userManager;

        public CustomerRepository(AppDataBaseContext dbContext,
            UserManager<Customer> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public async Task<IdentityResult> Create(Customer entity, string password) =>
            await userManager.CreateAsync(entity, password);
        public async Task Delete(Guid identifier) =>
            await userManager.DeleteAsync(await userManager.FindByIdAsync(identifier.ToString()));
        public async Task<Customer> FindById(Guid identifier) =>
            await userManager.FindByIdAsync(identifier.ToString());
        public async Task<Customer> FindByEmail(string email) =>
            await userManager.FindByEmailAsync(email);
        public async Task<List<Customer>> GetAll() =>
            await dbContext.Customers.ToListAsync();
        public void Update(Customer entity) =>
            dbContext.Entry(entity).State = EntityState.Modified;
    }
}
