using System;
using System.Net.Mail;
using Mediaverse.Application.Authentication.Services;

namespace Mediaverse.Infrastructure.Authentication.Services
{
    public class EmailService : IEmailService
    {
        public bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}