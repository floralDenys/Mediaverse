using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Domain.ContentSearch.Services
{
    public interface IQueryStringProcessor
    {
        ContentQueryType DefineQueryStringType(MediaContentSource source, string queryString);
    }
}