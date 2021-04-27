using System;

namespace Mediaverse.Application.Authentication.Services
{
    public interface INameGenerator
    {
        string GenerateAnonymousName();
        string GenerateAnonymousPassword();
    }
}