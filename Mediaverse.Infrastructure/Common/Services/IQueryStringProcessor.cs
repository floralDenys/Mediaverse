using Mediaverse.Domain.JointContentConsumption.Enums;
using Mediaverse.Infrastructure.Common.Enums;

namespace Mediaverse.Infrastructure.Common.Services
{
    public interface IQueryStringProcessor
    {
        QueryStringType DefineQueryStringType(MediaContentSource source, string queryString);
    }
}