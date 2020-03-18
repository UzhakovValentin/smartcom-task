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
            var item = new Item
            {
                Name = request.Name,
                Price = request.Price,
                Category = request.Category,
                Code = request.Code
            };
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

        [HttpPost("adduser")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
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

        [HttpPut("editeuser")]
        public async Task<IActionResult> EditUser([FromBody] Guid customerId)
        {
            return Ok();
        }

        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUser([FromBody] Guid customerId)
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
            return Json(orders.Where(order => order.Status == OrderStatus.New).Select(order => new
            {
                order.OrderId,
                order.OrderNumber,
                order.OrderDate,
                order.OrderElemnts
            }));
        }

        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmOrder([FromBody] ConfirmOrderRequest request)
        {
            var order = await repositoriesManager.Orders.Get(request.OrderId);

            order.Status = OrderStatus.InProccess;
            order.ShipmentDate = DateTime.Parse(request.ShipmentDate);

            await repositoriesManager.SaveChanges();
            return Ok();
        }

        [HttpPut("close")]
        public async Task<IActionResult> CloseOrder([FromBody] Guid orderId)
        {
            var order = await repositoriesManager.Orders.Get(orderId);
            order.Status = OrderStatus.Done;

            return Ok();
        }
    }
}