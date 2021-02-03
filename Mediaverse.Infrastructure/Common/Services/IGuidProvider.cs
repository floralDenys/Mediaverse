using System;

namespace Mediaverse.Infrastructure.Common.Services
{
    public interface IGuidProvider
    {
        Guid GetNewGuid();
    }
}