using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Domain.ContentSearch.Services
{
    public interface IQueryStringProcessor
    {
        QueryStringType DefineQueryStringType(MediaContentSource source, string queryString);
    }
}