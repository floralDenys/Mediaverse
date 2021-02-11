using System;
using System.Threading;
using System.Threading.Tasks;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Repositories
{
    public interface IRoomMemberRepository
    {
        Task<Viewer> GetViewerAsync(Guid memberId, CancellationToken cancellationToken);
    }
}