using System;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public abstract class Content : Entity
    {
        public string Title { get; }
        public string Description { get; }
        
        public ContentPlayer Player { get; }

        public Content(Guid id, string title, string description, ContentPlayer player) : base(id)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    throw new ArgumentException("Given title is invalid");
                }

                Title = title;
                Description = description;
                Player = player ?? throw new ArgumentException("Given player is invalid");
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create content", exception);
            }
        }
    }
}