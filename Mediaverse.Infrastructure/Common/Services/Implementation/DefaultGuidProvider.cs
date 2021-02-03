using System;

namespace Mediaverse.Infrastructure.Common.Services.Implementation
{
    public class DefaultGuidProvider : IGuidProvider
    {
        public Guid GetNewGuid() => Guid.NewGuid();
    }
}