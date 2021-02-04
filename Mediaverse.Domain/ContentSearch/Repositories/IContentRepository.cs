using System.Collections.Generic;
using Mediaverse.Domain.ContentSearch.Entities;
using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Domain.ContentSearch.Repositories
{
    public interface IContentRepository
    {
        IList<Preview> SearchForContent(MediaContentSource source, ContentQueryType contentQueryType, string queryString);
    }
}