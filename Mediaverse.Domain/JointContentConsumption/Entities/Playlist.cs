using System;
using System.Collections.Generic;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Playlist : List<Content>
    {
        public Guid Id { get; }
        
        public Host Owner { get; }
        public bool IsTemporary => Owner == null;
        
        public Playlist(Guid id, Host owner)
        {
            try
            {
                if (id == default)
                {
                    throw new ArgumentException("Given ID is invalid");
                }

                Id = id;
                Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create playlist", exception);
            }
        }
        
        public override string ToString() => $"{Id}";
    }
}