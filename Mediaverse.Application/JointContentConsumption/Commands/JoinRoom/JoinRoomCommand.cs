using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.CreateRoom.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Commands.JoinRoom
{
    public class JoinRoomCommand : IRequest<RoomDto>
    {
        public Guid ViewerId { get; set; }
        public Guid RoomId { get; set; }
    }
}