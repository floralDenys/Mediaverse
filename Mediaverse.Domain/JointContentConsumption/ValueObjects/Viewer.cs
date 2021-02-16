using System;
using Mediaverse.Domain.JointContentConsumption.Entities;

namespace Mediaverse.Domain.JointContentConsumption.ValueObjects
{
    public class Viewer : IEquatable<Viewer>
    {
        public UserProfile Profile { get; }

        public Viewer(UserProfile profile)
        {
            try
            {
                _ = profile ?? throw new ArgumentNullException(nameof(profile));
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create viewer", exception);
            }
        }

        public bool Equals(Viewer other)
        {
            if (other != null)
            {
                return other.Profile.Equals(Profile);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is Viewer other)
            {
                return Equals(other);
            }
            
            return false;
        }

        public override int GetHashCode() => Profile.GetHashCode();
    }
}