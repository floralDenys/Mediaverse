using System;
using MediatR;

namespace Mediaverse.Application.JointContentConsumption.Commands.ChangeContentPlayerState
{
    public class ChangeContentPlayerStateCommand : IRequest
    {
        public Guid RoomId { get; set; }
        public string State { get; set; }
        public long CurrentPlaybackTimeInSeconds { get; set; }
    }
}