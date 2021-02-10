using System;
using Mediaverse.Application.Common.Services;

namespace Mediaverse.Infrastructure.Common.Services
{
    public class DefaultGuidProvider : IGuidProvider
    {
        public Guid GetNewGuid() => Guid.NewGuid();
    }
}