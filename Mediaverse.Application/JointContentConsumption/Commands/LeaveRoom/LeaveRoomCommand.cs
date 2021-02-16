using System;
using MediatR;

namespace Mediaverse.Application.JointContentConsumption.Commands.LeaveRoom
{
    public class LeaveRoomCommand : IRequest
    {
        public Guid ViewerId { get; set; }
        public Guid RoomId { get; set; }
    }
}