using Mediaverse.Application.Authentication.Services;

namespace Mediaverse.Infrastructure.Authentication.Services
{
    public class NameGenerator : INameGenerator
    {
        public string GenerateAnonymousName() => "anonymous user";
    }
}