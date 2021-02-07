using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Application.JointContentConsumption.Commands.RemoveContentFromPlaylist
{
    public class RemoveContentFromPlaylistCommand : IRequest<PlaylistDto>
    {
        public Guid CurrentRoomId { get; set; }
        public ContentIdDto ContentId { get; set; }
        public MediaContentType ContentType { get; set; }
    }
}