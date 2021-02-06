using System;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Domain.JointContentConsumption.Factories
{
    public class ContentFactory
    {
        public Content CreateContent(ContentId contentId, MediaContentType contentType)
        {
            Content content;
            switch (contentType)
            {
                case MediaContentType.Video:
                    content = new Video(contentId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contentType));
            }

            return content;
        }
    }
}