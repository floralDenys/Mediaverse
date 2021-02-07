using System.Threading.Tasks;
using Mediaverse.Domain.JointContentConsumption.Entities;

namespace Mediaverse.Domain.JointContentConsumption.Repositories
{
    public interface IContentRepository
    {
        Task<Content> GetAsync(ContentId contentId);
    }
}