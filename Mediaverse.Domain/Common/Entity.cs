using System;

namespace Mediaverse.Domain.Common
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; }

        protected Entity(Guid id)
        {
            if (id == default)
            {
                throw new ArgumentException("Given ID is invalid");
            }

            Id = id;
        }

        public bool Equals(Entity other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is Entity other)
            {
                return Equals(other);
            }
            
            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => $"{Id.ToString()}";
    }
}