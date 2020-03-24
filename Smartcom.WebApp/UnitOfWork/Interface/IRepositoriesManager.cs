using Smartcom.WebApp.Models;
using Smartcom.WebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.UnitOfWork.Interface
{
    public interface IRepositoriesManager : IDisposable
    {
        ICustomerRepository<Customer> Customers { get; }
        IRepository<Item> Items { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderElement> OrderElements { get; }
        Task SaveChanges();
    }
}
