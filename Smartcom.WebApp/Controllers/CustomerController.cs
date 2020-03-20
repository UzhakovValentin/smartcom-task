using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartcom.WebApp.Models;
using Smartcom.WebApp.Services.Intefaces;
using Smartcom.WebApp.UnitOfWork;
using Smartcom.WebApp.ViewModels.Requests;

namespace Smartcom.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "Customer")]
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly RepositoriesManager repositoriesManager;
        private readonly IShoppingCartService<Order> shoppingCartService;

        public CustomerController(RepositoriesManager repositoriesManager,
            IShoppingCartService<Order> shoppingCartService)
        {
            this.repositoriesManager = repositoriesManager;
            this.shoppingCartService = shoppingCartService;
        }

        [HttpGet("allitems")]
        public async Task<IActionResult> GetAllItems()
        {
            return Json(await repositoriesManager.Items.GetAll());
        }

        [HttpGet("allorders/{customerId:guid}")]
        public async Task<IActionResult> GetCustomerOrders(Guid customerId)
        {
            var customer = await repositoriesManager.Customers.FindById(customerId);

            return Json(customer.Orders);
        }

        [HttpPost("addtocart/{customerId:guid}")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartRequest request, Guid customerId)
        {
            if (!CartExist())
            {
                var order = new Order
                {
                    OrderId = Guid.NewGuid(),
                    CustomerId = customerId,
                };
                shoppingCartService.AddToList(order);
                HttpContext.Response.Cookies.Append("OrderId", $"{order.OrderId}");
            }

            var orderId = Guid.Parse(HttpContext.Request.Cookies["OrderId"]);
            var item = await repositoriesManager.Items.Get(request.ItemId);

            var orderElement = new OrderElement
            {
                OrderElementId = Guid.NewGuid(),
                ItemId = request.ItemId,
                OrderId = orderId,
                ItemsCount = request.ItemsCount,
                ItemPrice = item.Price * request.ItemsCount
            };

            shoppingCartService.FindById(orderId).OrderElements.Add(orderElement);

            return Ok();
        }

        [HttpPost("makeorder")]
        public async Task<IActionResult> MakeOrder()
        {
            var orderId = Guid.Parse(Request.Cookies["OrderId"]);
            var order = shoppingCartService.FindById(orderId);

            order.OrderDate = DateTime.Now;
            order.Status = OrderStatuses.NEW;
            order.OrderNumber = 1234;

            await repositoriesManager.Orders.Create(order);
            await repositoriesManager.SaveChanges();
            Response.Cookies.Delete("OrderId");
            shoppingCartService.Remove(order);

            return Ok(new { order.OrderNumber });
        }

        [HttpGet("allorders/{customerId:guid}/{orderStatus}")]
        public async Task<IActionResult> GetOrderStatuses(Guid customerId, string orderStatus)
        {
            var customer = await repositoriesManager.Customers.FindById(customerId);

            if (StatusIsValid(orderStatus))
            {
                return Json(customer.Orders.Where(order => order.Status == orderStatus).ToList());
            }
            return NotFound();
        }

        [HttpDelete("deleteorder")]
        public async Task<IActionResult> DeleteOrder([FromBody] Guid orderId)
        {
            var order = await repositoriesManager.Orders.Get(orderId);

            if (order.Status == OrderStatuses.NEW)
            {
                await repositoriesManager.Orders.Delete(orderId);
                await repositoriesManager.SaveChanges();

                return Ok();
            }
            return BadRequest();
        }

        private bool StatusIsValid(string orderStatus)
        {
            if (orderStatus == OrderStatuses.NEW || orderStatus == OrderStatuses.IN_PROCESS || orderStatus == OrderStatuses.DONE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CartExist() =>
            HttpContext.Request.Cookies.ContainsKey("OrderId") &&
                shoppingCartService.FindById(Guid.Parse(HttpContext.Request.Cookies["OrderId"])) != null ? true : false;
    }
}