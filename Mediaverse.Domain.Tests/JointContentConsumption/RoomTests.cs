using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Room_update_selected_playlist(
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

        [Theory]
        [MemberData(nameof(GoodRoomsData))]
        public void Room_join(
            string roomName,
            Viewer host,
            RoomType type,
            Invitation invitation)
        {
            var actualViewer = new Viewer(
                new UserProfile(
                    id: Guid.NewGuid(),
                    name: "someusername",
                    isMember: false));
            
            var room = new Room(
                Guid.NewGuid(),
                roomName,
                host,
                type,
                invitation,
                Guid.NewGuid());
            
            room.Join(actualViewer);
            
            Assert.False(room.IsVacated());
            Assert.NotEmpty(room.Viewers);
            Assert.Equal(actualViewer, room.Viewers.First());
            Assert.NotEqual(actualViewer, room.Host);
        }
        
        [Theory]
        [MemberData(nameof(GoodRoomsData))]
        public void Room_leave(
            string roomName,
            Viewer host,
            RoomType type,
            Invitation invitation)
        {
            var viewer = new Viewer(
                new UserProfile(
                    id: Guid.NewGuid(),
                    name: "someusername",
                    isMember: false));
            
            var room = new Room(
                Guid.NewGuid(),
                roomName,
                host,
                type,
                invitation,
                Guid.NewGuid());
            
            room.Join(viewer);
            room.Leave(host);
            
            Assert.False(room.IsVacated());
            Assert.Empty(room.Viewers);
            Assert.Equal(viewer, room.Host);
            
            room.Leave(viewer);
            
            Assert.True(room.IsVacated());
            Assert.Empty(room.Viewers);
            Assert.Null(room.Host);
        }
        
        [Theory]
        [MemberData(nameof(GoodRoomsData))]
        public void Room_play_content(
            string roomName,
            Viewer host,
            RoomType type,
            Invitation invitation)
        {
            var actualContentId = new ContentId(
                "someExternalId",
                MediaContentSource.YouTube,
                MediaContentType.Video);
            
            var room = new Room(
                Guid.NewGuid(),
                roomName,
                host,
                type,
                invitation,
                Guid.NewGuid());
            
            room.PlayContent(actualContentId);
            
            Assert.Equal(actualContentId, room.CurrentContent.ContentId);
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