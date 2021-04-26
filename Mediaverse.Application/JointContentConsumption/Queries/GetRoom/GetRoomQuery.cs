using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetRoom
{
    public class GetRoomQuery : IRequest<RoomDto>
    {
        public Guid RoomId { get; set; }
    }
}