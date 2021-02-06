using System;
using Mediaverse.Domain.Common;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public abstract class Content
    {
        public ContentId Id { get; }
        public string Title { get; }
        public string Description { get; }
        
        public Content(ContentId id, string title, string description)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    throw new ArgumentException("Given title is invalid");
                }

                Id = id ?? throw new ArgumentNullException(nameof(id));
                Title = title;
                Description = description;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create content", exception);
            }
        }

        public Content(ContentId id)
        {
            try
            {
                Id = id ?? throw new ArgumentNullException(nameof(id));
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create content", exception);
            }
        }
    }
}