using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Commands.PlaySpecificContent
{
    public class PlaySpecificContentCommand : IRequest
    {
        public Guid RoomId { get; set; }
        public ContentIdDto ContentId { get; set; }
    }
}