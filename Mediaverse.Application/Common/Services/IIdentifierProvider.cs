using System;

namespace Mediaverse.Application.Common.Services
{
    public interface IIdentifierProvider
    {
        Guid GenerateGuid();
        string GenerateToken(Guid guid);
    }
}