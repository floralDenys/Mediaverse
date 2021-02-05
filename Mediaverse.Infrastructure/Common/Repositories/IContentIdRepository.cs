using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Infrastructure.Common.Services.Implementation;

namespace Mediaverse.Infrastructure.Common.Repositories
{
    public interface IContentIdRepository
    {
        ContentIdProvider.ContentId Get(string externalId, MediaContentSource source);
        void Save(ContentIdProvider.ContentId contentId);
    }
}