using System;
using MediatR;

namespace Mediaverse.Application.JointContentConsumption.Commands.SavePlaylist
{
    public class SavePlaylistCommand : IRequest
    {
        public Guid RoomId { get; set; }
        public Guid ViewerId { get; set; }
    }
}