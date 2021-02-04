using System;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Preview : Content
    {
        public Thumbnail Thumbnail { get; }
        
        public Preview(Guid videoId, string title, string description, Thumbnail thumbnail)
            : base(videoId, title, description)
        {
            try
            {
                Thumbnail = thumbnail ?? throw new ArgumentNullException(nameof(thumbnail));
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create preview", exception); 
            }
        }
    }
}