using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.PlayNextContent.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Commands.PlayNextContent
{
    public class PlayNextContentCommand : IRequest<ContentDto>
    {
        public Guid RoomId { get; set; }
    }
}