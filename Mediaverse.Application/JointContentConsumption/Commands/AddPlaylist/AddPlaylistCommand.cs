using System;
using MediatR;

namespace Mediaverse.Application.JointContentConsumption.Commands.AddPlaylist
{
    public class AddPlaylistCommand : IRequest
    {
        public Guid RoomId { get; set; }
        public Guid ViewerId { get; set; }
    }
}