using System;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public abstract class Content : IEquatable<Content>
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }
        
        public ContentPlayer Player { get; }

        public Content(Guid id, string title, string description, ContentPlayer player)
        {
            try
            {
                if (id == default)
                {
                    throw new ArgumentException("Given ID is invalid");
                }
                
                if (string.IsNullOrEmpty(title))
                {
                    throw new ArgumentException("Given title is invalid");
                }

                Id = id;
                Title = title;
                Description = description;
                Player = player ?? throw new ArgumentException("Given player is invalid");
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create content", exception);
            }
        }
        
        public bool Equals(Content other)
        {
            if (other == null)
            {
                return false;
            }
            
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is Content other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Title, Description, Player);
    }
}