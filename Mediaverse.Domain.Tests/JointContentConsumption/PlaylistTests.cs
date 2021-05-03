using System;
using System.Collections.Generic;
using System.Linq;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Enums;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Xunit;

namespace Mediaverse.Domain.Tests.JointContentConsumption
{
    public class PlaylistTests
    {
        [Fact]
        public void Create_playlist()
        {
            Guid actualPlaylistId = Guid.NewGuid();
            string actualName = "somename";
            var actualOwner = new Viewer(
                new UserProfile(
                    id: Guid.NewGuid(),
                    name: "somename",
                    isMember: true));
            
            var playlist = new Playlist(actualPlaylistId, actualName, actualOwner);
            
            Assert.Equal(actualPlaylistId, playlist.Id);
            Assert.Equal(actualName, playlist.Name);
            Assert.Equal(actualOwner, playlist.Owner);
        }
        
        [Fact]
        public void Switch_direction()
        {
            Guid actualPlaylistId = Guid.NewGuid();
            string actualName = "somename";
            var actualOwner = new Viewer(
                new UserProfile(
                    id: Guid.NewGuid(),
                    name: "somename",
                    isMember: true));
            var actualPlaylistItems = new List<PlaylistItem>
            {
                new PlaylistItem(new ContentId(
                    "someId",
                    MediaContentSource.YouTube,
                    MediaContentType.Video), 
                    1),
                new PlaylistItem(new ContentId(
                        "someId",
                        MediaContentSource.YouTube,
                        MediaContentType.Video), 
                    2),
            };

            var playlist = new Playlist(
                actualPlaylistId,
                actualName,
                actualOwner,
                actualPlaylistItems);

            Assert.Equal(
                actualPlaylistItems.First().ContentId, 
                playlist.GetNextContent(null));
            Assert.Equal(
                actualPlaylistItems.Last().ContentId, 
                playlist.GetNextContent(actualPlaylistItems.First().ContentId));
        }
    }
}