using System;
using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Domain.ContentSearch.Entities
{
    public class ContentId
    {
        public string ExternalId { get; }
        public MediaContentSource ContentSource { get; }
        public MediaContentType ContentType { get; }

        public ContentId(
            string id,
            MediaContentSource contentSource,
            MediaContentType contentType)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("External ID is null or empty");
                }
                
                ExternalId = id;
                ContentSource = contentSource;
                ContentType = contentType;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create external ID", exception);
            }
        }
    }
}