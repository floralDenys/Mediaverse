using System;
using System.Collections.Generic;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Enums;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Xunit;

namespace Mediaverse.Domain.Tests.JointContentConsumption
{
    public class RoomTests
    {
        [Fact]
        public void Room_create()
        {
            Guid roomId = Guid.NewGuid();
            string roomName = "some room name";
            var host = new Viewer(
                new UserProfile(
                    id: Guid.NewGuid(),
                    name: "Some name",
                    isMember: true));
            RoomType type = RoomType.Private;
            var invitation = new Invitation("token");
            Guid playlistId = Guid.NewGuid();

            var room = new Room(
                roomId,
                roomName,
                host,
                type,
                invitation,
                playlistId);
            
            Assert.Equal(roomId, room.Id);
            Assert.Equal(roomName, room.Name);
            Assert.Equal(host, room.Host);
            Assert.Equal(type, room.Type);
            Assert.Equal(invitation.Token, room.Invitation.Token);
            Assert.Equal(playlistId, room.ActivePlaylistId);
        }

        [Theory]
        [MemberData(nameof(GoodRoomsData))]
        public void Room_update_selected_playlist_success(
            string roomName,
            Viewer host,
            RoomType type,
            Invitation invitation)
        {
            var oldPlaylistId = Guid.NewGuid();
            var newPlaylistId = Guid.NewGuid();
            
            var room = new Room(
                Guid.NewGuid(),
                roomName,
                host,
                type,
                invitation,
                oldPlaylistId);
            
            var playlist = new Playlist(newPlaylistId, "some name", host);
            
            room.UpdateSelectedPlaylist(playlist);
            
            Assert.Equal(playlist.Id, room.ActivePlaylistId);
        }

        public static IEnumerable<object []> GoodRoomsData()
        {
            yield return new object[]
            {
                "some room name",
                new Viewer(
                    new UserProfile(
                        id: Guid.NewGuid(),
                        name: "Some name",
                        isMember: true)),
                RoomType.Private,
                new Invitation("token")
            };
        }
    }
}