using Smartcom.WebApp.Database;
using Smartcom.WebApp.Repositories;
using System;

namespace Smartcom.WebApp.UnitOfWork
{
    public class RepositoriesManager : IDisposable
    {
        private readonly AppDataBaseContext dbContext;
        private CustomerRepository customerRepository;
        private ItemRepository itemRepository;
        private OrderRepository orderRepository;
        private OrderElementRepository orderElementRepository;
        private bool isDisposed = false;

        public RepositoriesManager(AppDataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public CustomerRepository Customers
        {
            get
            {
                if (customerRepository == null)
                {
                    customerRepository = new CustomerRepository(dbContext);
                }
                return customerRepository;
            }
        }
        public ItemRepository Items
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
        public OrderRepository Orders
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
        public OrderElementRepository OrderElements
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
