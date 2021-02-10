using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.CreateRoom.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Commands.CreateRoom
{
    public class CreateRoomCommand : IRequest<RoomDto>
    {
        public string Name { get; set; }
        public Guid HostId { get; set; }
    }
}