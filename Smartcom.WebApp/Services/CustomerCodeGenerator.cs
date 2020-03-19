using Smartcom.WebApp.Services.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Services
{
    public class CustomerCodeGenerator : ICustomerCodeGenerator
    {
        private int counter = 0;

        public string GenerateCode()
        {
            var registrationYear = DateTime.Now.Year;
            var code = $"{counter}-{registrationYear}";

            counter++;
            return code;
        }
    }
}
