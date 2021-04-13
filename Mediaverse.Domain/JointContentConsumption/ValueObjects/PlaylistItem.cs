using System;

namespace Mediaverse.Domain.JointContentConsumption.ValueObjects
{
    public class PlaylistItem
    {
        public ContentId ContentId { get; }
        public int PlaylistItemIndex { get; }

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
    }
}