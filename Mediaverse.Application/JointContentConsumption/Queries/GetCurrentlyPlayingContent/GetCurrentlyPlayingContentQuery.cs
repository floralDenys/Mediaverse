using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.SwitchContent.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetCurrentlyPlayingContent
{
    public class GetCurrentlyPlayingContentQuery : IRequest<ContentDto>
    {
        public Guid RoomId { get; set; }
    }
}