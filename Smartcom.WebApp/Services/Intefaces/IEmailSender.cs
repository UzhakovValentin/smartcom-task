using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Services.Intefaces
{
    public interface IEmailSender
    {
        Task SendEmail(string email, string subject, string message);
    }
}
