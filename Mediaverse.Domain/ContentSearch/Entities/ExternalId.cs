using System;
using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Domain.ContentSearch.Entities
{
    public class ExternalId
    {
        public string Id { get; }
        public MediaContentSource ContentSource { get; }
        public ContentType ContentType { get; }

        public ExternalId(
            string id,
            MediaContentSource contentSource,
            ContentType contentType)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("External ID is null or empty");
                }
                
                Id = id;
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