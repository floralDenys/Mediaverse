using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Application.JointContentConsumption.Commands.CreateRoom
{
    public class CreateRoomCommand : IRequest<RoomDto>
    {
        public Guid HostId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RoomType Type { get; set; }
    }
}