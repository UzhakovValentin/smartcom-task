using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartcom.WebApp.Models;
using Smartcom.WebApp.UnitOfWork;

namespace Smartcom.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "Customer")]
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly RepositoriesManager repositoriesManager;

        public CustomerController(RepositoriesManager repositoriesManager)
        {
            this.repositoriesManager = repositoriesManager;
        }

        [HttpGet("allitems")]
        public async Task<IActionResult> GetAllItems()
        {
            return Json(await repositoriesManager.Items.GetAll());
        }

        [HttpGet("allorders/{customerId:guid}")]
        public async Task<IActionResult> GetCustomersOrders(Guid customerId)
        {
            var customer = await repositoriesManager.Customers.FindById(customerId);

            return Json(customer.Orders);
        }

        [HttpGet("allorders/{customerId:guid}/{orderStatus}")]
        public async Task<IActionResult> GetOrderStatuses(Guid customerId, string orderStatus)
        {
            var customer = await repositoriesManager.Customers.FindById(customerId);

            return Json(customer.Orders.Where(order => order.Status == orderStatus).ToList());
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
    }
}