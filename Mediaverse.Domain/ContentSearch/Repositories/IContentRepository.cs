using System.Threading.Tasks;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.ValueObjects;

namespace Mediaverse.Domain.ContentSearch.Repositories
{
    public interface IContentRepository
    {
        Task<SearchResult> SearchForContent(MediaContentSource source, ContentQueryType contentQueryType, string queryString);
    }
}