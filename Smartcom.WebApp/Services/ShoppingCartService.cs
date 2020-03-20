using Smartcom.WebApp.Models;
using Smartcom.WebApp.Services.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Services
{
    public class ShoppingCartService : IShoppingCartService<Order>
    {
        private List<Order> orders = new List<Order>();

        public void AddToList(Order order) =>
            orders.Add(order);

        public Order FindById(Guid id) =>
            orders.Find(order => order.OrderId == id);

        public Order FindByCustomerId(Guid customerId) =>
            orders.Find(order => order.CustomerId == customerId);

        public void Remove(Order order) =>
            orders.Remove(order);
    }
}
