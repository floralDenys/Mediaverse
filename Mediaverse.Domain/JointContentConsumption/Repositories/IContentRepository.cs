using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Domain.JointContentConsumption.Repositories
{
    public interface IContentRepository
    {
        Content SearchForContent(MediaContentSource source, string queryString);
    }
}