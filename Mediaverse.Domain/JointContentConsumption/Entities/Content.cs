using System;
using Mediaverse.Domain.Common;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public abstract class Content : Entity
    {
        public string Title { get; }
        public string Description { get; }
        
        public Content(Guid id, string title, string description) : base(id)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    throw new ArgumentException("Given title is invalid");
                }

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