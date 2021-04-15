using System;
using Mediaverse.Application.Common.Services;

namespace Mediaverse.Infrastructure.Common.Services
{
    public class DefaultIdentifierProvider : IIdentifierProvider
    {
        public Guid GenerateGuid() => Guid.NewGuid();
        public string GenerateToken(Guid guid) =>
            Convert.ToBase64String(guid.ToByteArray())
                .Replace("+","")
                .Replace("=","");
    }
}