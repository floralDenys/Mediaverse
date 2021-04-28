using System;
using Mediaverse.Application.JointContentConsumption.Commands.SwitchContent.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Common.Dtos
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ActivePlaylistId { get; set; }
        public ContentDto CurrentlyPlayingContent { get; set; }
    }
}