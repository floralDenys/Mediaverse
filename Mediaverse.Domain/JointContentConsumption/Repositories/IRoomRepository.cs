﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Mediaverse.Domain.JointContentConsumption.Entities;

namespace Mediaverse.Domain.JointContentConsumption.Repositories
{
    public interface IRoomRepository
    {
        Task<Room> GetAsync(Guid roomId, CancellationToken cancellationToken);
        Task SaveAsync(Room room, CancellationToken cancellationToken);
    }
}