using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Smartcom.WebApp.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Auth
{
    public static class Authentication
    {
        public static async Task Authenticate(Customer customer, UserManager<Customer> userManager, HttpContext httpContext)
        {
            List<Claim> claims = new List<Claim>();
            var userRoles = await userManager.GetRolesAsync(customer);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole));
            }

            Claim claim = new Claim(ClaimsIdentity.DefaultNameClaimType, customer.Email);
            claims.Add(claim);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }
    }
}
