using System.Threading.Tasks;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Repositories
{
    public interface IContentRepository
    {
        Task<Content> GetAsync(ContentId contentId);
    }
}