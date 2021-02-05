using System;
using Mediaverse.Domain.ContentSearch.Entities;
using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Domain.ContentSearch.ValueObjects
{
    public class Preview
    {
        public ExternalId ExternalId { get; }
        public string ContentTitle { get; }
        public string ContentDescription { get; }
        public Thumbnail Thumbnail { get; set; }
        
        public Preview(
            string externalId,
            MediaContentSource contentSource,
            ContentType contentType,
            string contentTitle,
            string contentDescription,
            Thumbnail thumbnail)
        {
            try
            {
                if (string.IsNullOrEmpty(contentTitle))
                {
                    throw new ArgumentException("Given title is null or empty");
                }
                
                ExternalId = new ExternalId(externalId, contentSource, contentType);
                ContentTitle = contentTitle;
                ContentDescription = contentDescription;
                Thumbnail = thumbnail ?? throw new ArgumentNullException(nameof(thumbnail));
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create preview", exception); 
            }
        }
    }
}