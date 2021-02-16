using System;
using System.Collections.Generic;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Commands.DeletePlaylist
{
    public class DeletePlaylistCommand : IRequest<IList<PlaylistDto>>
    {
        public Guid MemberId { get; set; }
        public Guid PlaylistId { get; set; }
    }
}