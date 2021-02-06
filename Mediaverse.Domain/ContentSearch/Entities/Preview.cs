﻿using System;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.ValueObjects;

namespace Mediaverse.Domain.ContentSearch.Entities
{
    public class Preview
    {
        public ContentId ContentId { get; }
        public string ContentTitle { get; }
        public string ContentDescription { get; }
        public Thumbnail Thumbnail { get; set; }
        
        public Preview(
            string externalId,
            MediaContentSource contentSource,
            MediaContentType contentType,
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
                
                ContentId = new ContentId(externalId, contentSource, contentType);
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