using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Commands.JoinRoom
{
    public class JoinRoomCommand : IRequest<RoomDto>
    {
        public Guid ViewerId { get; set; }
        public string RoomToken { get; set; }
    }
}