using System;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist
{
    public class GetPlaylistQuery : IRequest<PlaylistDto>
    {
        public Guid RoomId { get; set; }
    }
}