using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Smartcom.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "Customer")]
    [Route("customer")]
    public class CustomerController : Controller
    {

    }
}