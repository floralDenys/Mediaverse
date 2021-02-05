using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Infrastructure.Common.Services.Implementation;

namespace Mediaverse.Infrastructure.Common.Repositories
{
    public interface IContentIdRepository
    {
        ContentIdProvider.ContentId GetContentId(string externalId, MediaContentSource source);
        void SaveContentId(ContentIdProvider.ContentId contentId);
    }
}