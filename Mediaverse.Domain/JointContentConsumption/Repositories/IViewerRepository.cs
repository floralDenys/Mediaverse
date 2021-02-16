using System;
using System.Threading;
using System.Threading.Tasks;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Repositories
{
    public interface IViewerRepository
    {
        Task<Viewer> GetAsync(Guid memberId, CancellationToken cancellationToken);
    }
}