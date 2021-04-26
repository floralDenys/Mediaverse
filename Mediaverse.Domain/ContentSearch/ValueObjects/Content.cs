using System;
using Mediaverse.Domain.ContentSearch.Entities;

namespace Mediaverse.Domain.ContentSearch.ValueObjects
{
    public class Content
    {
        public ContentId Id { get; }

        public string Title { get; }
        public string Description { get; }
        
        public string PlayerHtml { get; }
        public int PlayerWidth { get; }
        public int PlayerHeight { get; }
        
        public Thumbnail Thumbnail { get; }

        public Content(
            ContentId id,
            string title,
            Thumbnail thumbnail,
            string playerHtml,
            int playerWidth,
            int playerHeight,
            string description = "")
        {
            if (string.IsNullOrEmpty(playerHtml))
            {
                throw new ArgumentException("Player Html must be provided");
            }

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Title must be provided");
            }
            
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title;
            Thumbnail = thumbnail;
            Description = description;
            PlayerHtml = playerHtml;
            PlayerWidth = playerWidth;
            PlayerHeight = playerHeight; 
        }
    }
}