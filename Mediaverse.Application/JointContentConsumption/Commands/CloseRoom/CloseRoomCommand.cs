using System;
using MediatR;

namespace Mediaverse.Application.JointContentConsumption.Commands.CloseRoom
{
    public class CloseRoomCommand : IRequest
    {
        public Guid RoomId { get; set; }
        public Guid MemberId { get; set; }
    }
}