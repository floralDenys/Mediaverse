using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Domain.JointContentConsumption.Repositories
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetRooms(RoomType type, CancellationToken cancellationToken);
        Task<Room> GetAsync(Guid roomId, CancellationToken cancellationToken);
        Task<Room> GetAsync(string roomToken, CancellationToken cancellationToken);
        Task AddAsync(Room room, CancellationToken cancellationToken);
        Task UpdateAsync(Room room, CancellationToken cancellationToken);
        Task DeleteAsync(Guid roomId, CancellationToken cancellationToken);
    }
}