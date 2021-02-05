using System;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.ValueObjects;

namespace Mediaverse.Domain.ContentSearch.Entities
{
    public class Preview
    {
        public Guid ContentId { get; }
        public ContentType ContentType { get; }
        public string ContentTitle { get; }
        public string ContentDescription { get; }
        public Thumbnail Thumbnail { get; set; }
        
        public Preview(
            Guid contentId,
            ContentType contentType,
            string contentTitle,
            string contentDescription,
            Thumbnail thumbnail)
        {
            try
            {
                if (contentId == default)
                {
                    throw new ArgumentException("Given ID is invalid");    
                }

                if (string.IsNullOrEmpty(contentTitle))
                {
                    throw new ArgumentException("Given title is null or empty");
                }

                ContentId = contentId;
                ContentType = contentType;
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