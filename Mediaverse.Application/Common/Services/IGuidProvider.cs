using System;

namespace Mediaverse.Application.Common.Services
{
    public interface IGuidProvider
    {
        Guid GetNewGuid();
    }
}