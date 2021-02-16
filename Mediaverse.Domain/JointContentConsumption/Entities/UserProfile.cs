using System;
using Mediaverse.Domain.Common;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class UserProfile : Entity, IEquatable<UserProfile>
    {
        public string Name { get; }
        public bool IsResident { get; }

        public UserProfile(Guid id, string name, bool isResident) : base(id)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Given name is invalid");
                }

                Name = name;
                IsResident = isResident;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create user profile", exception);
            }
        }

        public bool Equals(UserProfile other)
        {
            if (other != null)
            {
                return other.Id == Id;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is UserProfile other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}