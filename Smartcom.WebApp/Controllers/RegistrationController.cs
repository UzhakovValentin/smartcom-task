using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smartcom.WebApp.Auth;
using Smartcom.WebApp.Models;
using Smartcom.WebApp.UnitOfWork;
using Smartcom.WebApp.ViewModels.Requests;
using Smartcom.WebApp.ViewModels.Responces;

namespace Smartcom.WebApp.Controllers
{
    [Route("registration")]
    public class RegistrationController : Controller
    {
        private readonly RepositoriesManager repositoriesManager;
        public readonly UserManager<Customer> userManager;

        public RegistrationController(RepositoriesManager repositoriesManager,
            UserManager<Customer> userManager)
        {
            this.repositoriesManager = repositoriesManager;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequest request)
        {
            if (ModelState.IsValid)
            {
                if (await repositoriesManager.Customers.FindByEmail(request.Email) == null)
                {
                    var customer = new Customer
                    {
                        Name = request.Name,
                        Address = request.Address,
                        Email = request.Email,
                        UserName = request.Email,
                        Discount = default,
                        Code = GenerateCustomerCode()
                    };
                    var registrationResult = await repositoriesManager.Customers.Create(customer, request.Password);

                    if (!registrationResult.Succeeded)
                    {
                        return BadRequest(registrationResult.Errors);
                    }
                    await userManager.AddToRoleAsync(customer, "Customer");
                    await Authentication.Authenticate(customer, userManager, HttpContext);
                    return Ok(new CustomerIdResponce { CustomerId = customer.Id });
                }
                return BadRequest("Email has been already taken");
            }
            return BadRequest("Request model is invalid");
        }

        private string GenerateCustomerCode()
        {
            var registrationDay = DateTime.Now.Day;
            var registrationMounth = DateTime.Now.Month;
            var registrationYear = DateTime.Now.Year;
            return $"{registrationDay}{registrationMounth}-{registrationYear}";
        }
    }
}