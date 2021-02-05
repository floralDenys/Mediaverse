using System;
using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Infrastructure.Common.Services
{
    public interface IContentIdProvider
    {
        Guid GetOrCreateInternalId(string externalId, MediaContentSource source);
    }
}