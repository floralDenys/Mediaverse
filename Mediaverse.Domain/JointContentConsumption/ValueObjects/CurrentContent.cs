using System;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Domain.JointContentConsumption.ValueObjects
{
    public class CurrentContent
    {
        public ContentId ContentId { get; }
        public ContentPlayerState PlayerState { get; set; }
        private double _playingTime;

        public double PlayingTime
        {
            get => _playingTime
                   + (PlayerState == ContentPlayerState.Playing 
                   ? (LastUpdatedPlayingTime - DateTime.Now).Seconds
                   : 0L);
            set => _playingTime = value;
        }
        public DateTime LastUpdatedPlayingTime { get; set; }

        public CurrentContent(
            ContentId contentId,
            ContentPlayerState playerState,
            double playingTime,
            DateTime lastUpdatedPlayingTime)
        {
            ContentId = contentId;
            PlayerState = playerState;
            PlayingTime = playingTime;
            LastUpdatedPlayingTime = lastUpdatedPlayingTime;
        }
    }
}