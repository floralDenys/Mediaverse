using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Application.JointContentConsumption.Commands.SwitchContent
{
    public class SwitchContentCommand : IRequest<AffectedViewers>
    {
        public Guid RoomId { get; set; }
        public SwitchContentDirection Direction { get; set; }
    }
}