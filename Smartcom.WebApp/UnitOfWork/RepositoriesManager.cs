using Microsoft.AspNetCore.Identity;
using Smartcom.WebApp.Database;
using Smartcom.WebApp.Models;
using Smartcom.WebApp.Repositories;
using Smartcom.WebApp.Repositories.Interfaces;
using Smartcom.WebApp.UnitOfWork.Interface;
using System;
using System.Threading.Tasks;

namespace Smartcom.WebApp.UnitOfWork
{
    public class RepositoriesManager : IRepositoriesManager
    {
        private readonly AppDataBaseContext dbContext;
        private readonly UserManager<Customer> userManager;
        private ICustomerRepository<Customer> customerRepository;
        private IRepository<Item> itemRepository;
        private IRepository<Order> orderRepository;
        private IRepository<OrderElement> orderElementRepository;
        private bool isDisposed = false;

        public RepositoriesManager(AppDataBaseContext dbContext,
            UserManager<Customer> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public ICustomerRepository<Customer> Customers
        {
            get
            {
                if (customerRepository == null)
                {
                    customerRepository = new CustomerRepository(dbContext, userManager);
                }
                return customerRepository;
            }
        }
        public virtual IRepository<Item> Items
        {
            get
            {
                if (itemRepository == null)
                {
                    itemRepository = new ItemRepository(dbContext);
                }
                return itemRepository;
            }
        }
        public IRepository<Order> Orders
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new OrderRepository(dbContext);
                }
                return orderRepository;
            }
        }
        public IRepository<OrderElement> OrderElements
        {
            get
            {
                if (orderElementRepository == null)
                {
                    orderElementRepository = new OrderElementRepository(dbContext);
                }
                return orderElementRepository;
            }
        }

        public async Task SaveChanges() =>
            await dbContext.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
                isDisposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
