using System;
using System.Collections.Generic;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetAvailablePlaylists
{
    public class GetAvailablePlaylistsQuery : IRequest<IList<PlaylistDto>>
    {
        public Guid RoomId { get; set; }
    }
}