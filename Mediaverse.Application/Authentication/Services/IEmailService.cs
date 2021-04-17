namespace Mediaverse.Application.Authentication.Services
{
    public interface IEmailService
    {
        bool IsValidEmail(string email);
    }
}