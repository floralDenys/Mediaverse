using System;

namespace Mediaverse.Domain.Common
{
    public abstract class Entity
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

        public override string ToString() => $"{Id.ToString()}";
    }
}