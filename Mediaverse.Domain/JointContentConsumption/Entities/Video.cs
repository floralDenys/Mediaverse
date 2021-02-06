using System;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Video : Content
    {
        public ContentPlayer Player { get; }

        public Video(ContentId id, string title, string description, ContentPlayer player) 
            : base(id, title, description)
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

        public Video(ContentId id) : base(id) { }
    }
}