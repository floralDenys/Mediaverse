using System;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public abstract class Content
    {
        public ContentId Id { get; }
        public string Title { get; }
        public string Description { get; }
        
        protected Content(ContentId id, string title, string description = "")
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
    }
}