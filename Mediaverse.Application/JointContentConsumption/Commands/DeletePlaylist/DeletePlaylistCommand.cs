using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Commands.DeletePlaylist
{
    public class DeletePlaylistCommand : IRequest<AffectedViewersDto>
    {
        public Guid MemberId { get; set; }
        public Guid RoomId { get; set; }
    }
}