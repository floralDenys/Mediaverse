using System;
using System.Collections.Generic;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetAvailablePlaylists
{
    public class GetAvailablePlaylistsQuery : IRequest<IList<SelectablePlaylistDto>>
    {
        public Guid HostId { get; set; }
    }
}