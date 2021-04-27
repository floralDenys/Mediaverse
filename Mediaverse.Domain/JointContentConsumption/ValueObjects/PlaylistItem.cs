using System;
using Mediaverse.Domain.ContentSearch.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.ValueObjects
{
    public class PlaylistItem
    {
        public ContentId ContentId { get; }
        public int PlaylistItemIndex { get; }
        
        public string Title { get; }
        public string Description { get; }

        public PlaylistItem(
            ContentId contentId,
            int playlistItemIndex)
        {
            ContentId = contentId ?? throw new ArgumentNullException(nameof(contentId));

            if (playlistItemIndex <= 0)
            {
                throw new ArgumentException("Playlist item index should be higher than 0");
            }

            PlaylistItemIndex = playlistItemIndex;
        }
        
        public PlaylistItem(
            ContentId contentId,
            int playlistItemIndex,
            string title,
            string description)
        {
            ContentId = contentId ?? throw new ArgumentNullException(nameof(contentId));

            if (playlistItemIndex <= 0)
            {
                throw new ArgumentException("Playlist item index should be higher than 0");
            }

            PlaylistItemIndex = playlistItemIndex;

            Title = title;
            Description = description;
        }
    }
}