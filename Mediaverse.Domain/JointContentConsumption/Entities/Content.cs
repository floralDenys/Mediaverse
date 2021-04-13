using System;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Content
    {
        public ContentId Id { get; }
        public string Title { get; }
        public string Description { get; }
        public ContentPlayer Player { get; }

        public Content(ContentId id, string title, ContentPlayer player, string description = "")
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    throw new ArgumentException("Given title is invalid");
                }

                Id = id ?? throw new ArgumentNullException(nameof(id));
                Title = title;
                Player = player ?? throw new ArgumentException("Given player is invalid");
                Description = description;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create content", exception);
            }
        }
    }
}