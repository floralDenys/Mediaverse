using System;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Domain.JointContentConsumption.ValueObjects
{
    public class ContentId : IEquatable<ContentId>
    {
        public string ExternalId { get; }
        public MediaContentSource ContentSource { get; } 
        public MediaContentType ContentType { get; }
        public int PlaylistItemIndex { get; }

        public ContentId(
            string externalId,
            MediaContentSource contentSource,
            MediaContentType contentType)
        {
            try
            {
                if (string.IsNullOrEmpty(externalId))
                {
                    throw new ArgumentException("External ID is null or empty");
                }

                ExternalId = externalId;
                ContentSource = contentSource;
                ContentType = contentType;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create content ID", exception);
            }
        }
        
        public bool Equals(ContentId other)
        {
            if (other == null)
            {
                return false;
            }
            
            return ExternalId == other.ExternalId 
                   && ContentSource == other.ContentSource
                   && ContentType == other.ContentType;
        }

        public override bool Equals(object obj)
        {
            if (obj is ContentId other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(ExternalId, (int) ContentSource, (int) ContentType);
    }
}