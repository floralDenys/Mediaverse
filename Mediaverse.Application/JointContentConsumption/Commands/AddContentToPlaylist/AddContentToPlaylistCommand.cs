using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Commands.AddContentToPlaylist
{
    public class AddContentToPlaylistCommand : IRequest<AffectedViewers>
    {
        public Guid CurrentRoomId { get; set; }
        public ContentIdDto ContentId { get; set; }
    }
}