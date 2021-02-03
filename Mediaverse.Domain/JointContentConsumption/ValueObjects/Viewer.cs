using System;
using Mediaverse.Domain.JointContentConsumption.Entities;

namespace Mediaverse.Domain.JointContentConsumption.ValueObjects
{
    public class Viewer
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
    }
}