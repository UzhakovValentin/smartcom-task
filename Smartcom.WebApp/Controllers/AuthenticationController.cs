using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("authentication")]
    public class AuthenticationController : Controller
    {
        private readonly RepositoriesManager repositoriesManager;
        private readonly UserManager<Customer> userManager;

        public AuthenticationController(RepositoriesManager repositoriesManager,
            UserManager<Customer> userManager)
        {
            this.repositoriesManager = repositoriesManager;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var customer = await repositoriesManager.Customers.FindByEmail(request.Email);
                if (customer != null && await userManager.CheckPasswordAsync(customer, request.Password))
                {
                    await Authentication.Authenticate(customer, userManager, HttpContext);
                    return Ok();
                }
                return BadRequest("Wrong login or password");
            }
            return BadRequest("Request model is invalid");
        }
    }
}