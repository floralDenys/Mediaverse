using System;
using System.Collections.Generic;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Commands.ChangeContentPlayerState
{
    public class ChangeContentPlayerStateCommand : IRequest<AffectedViewers>
    {
        public Guid RoomId { get; set; }
        public string State { get; set; }
        public double CurrentPlaybackTimeInSeconds { get; set; }
    }
}