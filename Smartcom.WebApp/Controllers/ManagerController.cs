using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smartcom.WebApp.Models;
using Smartcom.WebApp.Services.Intefaces;
using Smartcom.WebApp.UnitOfWork;
using Smartcom.WebApp.ViewModels.Requests;

namespace Smartcom.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "Manager")]
    [Route("manager")]
    public class ManagerController : Controller
    {
        private readonly RepositoriesManager repositoriesManager;
        private readonly UserManager<Customer> userManager;
        private readonly IEmailSender emailSender;
        private readonly IPasswordGenerator passwordGenerator;

        public ManagerController(RepositoriesManager repositoriesManager,
            UserManager<Customer> userManager,
            IEmailSender emailSender,
            IPasswordGenerator passwordGenerator)
        {
            this.repositoriesManager = repositoriesManager;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.passwordGenerator = passwordGenerator;
        }

        [HttpGet("allitems")]
        public async Task<IActionResult> GetAllItems()
        {
            return Json(await repositoriesManager.Items.GetAll());
        }

        [HttpGet("item/{itemId:guid}")]
        public async Task<IActionResult> GetItem(Guid itemId)
        {
            return Json(await repositoriesManager.Items.Get(itemId));
        }

        [HttpPost("additem")]
        public async Task<IActionResult> AddItem([FromBody] AddItemRequest request)
        {
            if (ModelState.IsValid)
            {
                var item = new Item
                {
                    Name = request.Name,
                    Price = request.Price,
                    Code = request.Code,
                    Category = request.Category
                };
                await repositoriesManager.Items.Create(item);
                await repositoriesManager.SaveChanges();

                return Ok(new { item.ItemId });
            }
            return BadRequest("Request model is invalid");
        }

        [HttpPut("edititem")]
        public async Task<IActionResult> EditItem([FromBody] EditItemRequest request)
        {
            var item = await repositoriesManager.Items.Get(request.EditedItemId);

            item.Name = request.Name;
            item.Price = request.Price;
            item.Category = request.Category;
            item.Code = request.Code;

            repositoriesManager.Items.Update(item);
            await repositoriesManager.SaveChanges();
            return Ok();
        }

        [HttpDelete("deleteitem")]
        public async Task<IActionResult> DeleteItem([FromBody] Guid itemId)
        {
            await repositoriesManager.Items.Delete(itemId);
            await repositoriesManager.SaveChanges();

            return Ok();
        }

        [HttpGet("allcustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            return Json(await repositoriesManager.Customers.GetAll());
        }

        [HttpGet("customer/{customerId:guid}")]
        public async Task<IActionResult> GetCustomer(Guid customerId)
        {
            return Json(await repositoriesManager.Customers.FindById(customerId));
        }

        [HttpPost("addcustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] AddUserRequest request)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Address = request.Address,
                Discount = request.Discount,
                Email = request.Email,
                UserName = request.Email
            };
            var password = passwordGenerator.GaneratePassword();
            var emailSubject = "";
            var emailMessage = $"Your password: {password}";
            var result = await repositoriesManager.Customers.Create(customer, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(customer, "Customer");
                await emailSender.SendEmail(request.Email, emailSubject, emailMessage);
                return Ok(new { CustomerId = customer.Id });
            }
            return BadRequest();
        }

        [HttpPut("editcustomer")]
        public async Task<IActionResult> EditCustomer([FromBody] EditUserRequest request)
        {
            var customer = await userManager.FindByIdAsync(request.EditedUserId.ToString());

            customer.Name = request.Name;
            customer.Address = request.Address;
            customer.Discount = request.Discount;

            await userManager.UpdateAsync(customer);
            return Ok();
        }

        [HttpDelete("deletecustomer")]
        public async Task<IActionResult> DeleteCustomer([FromBody] Guid customerId)
        {
            var customer = await repositoriesManager.Customers.FindById(customerId);
            var emailSubject = "";
            var emailMessage = "You have been deleted";

            await userManager.DeleteAsync(customer);
            await emailSender.SendEmail(customer.Email, emailSubject, emailMessage);

            return Ok();
        }

        [HttpGet("neworders")]
        public async Task<IActionResult> GetNewOrders()
        {
            var orders = await repositoriesManager.Orders.GetAll();
            return Json(orders.Where(order => order.Status == OrderStatuses.NEW).Select(order => new
            {
                order.OrderId,
                order.OrderNumber,
                order.OrderDate,
                order.OrderElements
            }));
        }

        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmOrder([FromBody] ConfirmOrderRequest request)
        {
            var order = await repositoriesManager.Orders.Get(request.OrderId);

            order.Status = OrderStatuses.IN_PROCESS;
            order.ShipmentDate = DateTime.Parse(request.ShipmentDate);

            await repositoriesManager.SaveChanges();
            return Ok();
        }

        [HttpPut("close")]
        public async Task<IActionResult> CloseOrder([FromBody] Guid orderId)
        {
            var order = await repositoriesManager.Orders.Get(orderId);
            order.Status = OrderStatuses.DONE;

            return Ok();
        }
    }
}