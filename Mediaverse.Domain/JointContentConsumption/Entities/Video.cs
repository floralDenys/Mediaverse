using System;
using Mediaverse.Domain.JointContentConsumption.Enums;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Video : Content
    {
        public ContentPlayer Player { get; }

        public Video(
            string externalId,
            MediaContentSource contentSource,
            ContentPlayer player,
            string title,
            string description = ""
            ) : base(new ContentId(externalId, contentSource, MediaContentType.Video), title, description)
        {
            try
            {
                Player = player ?? throw new ArgumentException("Given player is invalid");
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create video", exception);
            }
        }
    }
}