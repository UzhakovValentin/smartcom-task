using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Smartcom.WebApp.Services.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmail(string email, string subject, string message)
        {
            var adminEmail = configuration.GetSection("EmailSettings")["Email"];
            var password = configuration.GetSection("EmailSettings")["Password"];
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация интернет-магазина", adminEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 25, true);
                await client.AuthenticateAsync(adminEmail, password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
