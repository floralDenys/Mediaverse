using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.ValueObjects;
using Mediaverse.Domain.JointContentConsumption.Entities;

namespace Mediaverse.Domain.ContentSearch.Repositories
{
    public interface IContentRepository
    {
        SearchResult SearchForContent(MediaContentSource source, string queryString);
    }
}