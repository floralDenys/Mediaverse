using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.SwitchContent.Dtos;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Application.JointContentConsumption.Commands.SwitchContent
{
    public class SwitchContentCommand : IRequest<ContentDto>
    {
        public Guid RoomId { get; set; }
        public SwitchContentDirection Direction { get; set; }
    }
}