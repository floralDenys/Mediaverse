using System;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class UserProfile : IEquatable<UserProfile>
    {
        public Guid Id { get; }
        public string Name { get; }
        public bool IsResident { get; }

        public UserProfile(Guid id, string name, bool isResident)
        {
            try
            {
                if (id == default)
                {
                    throw new ArgumentException("Given ID is invalid");
                }

                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Given name is invalid");
                }

                Id = id;
                Name = name;
                IsResident = isResident;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create user profile");
            }
        }

        public bool Equals(UserProfile other)
        {
            if (other == null)
            {
                return false;
            }
            
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is UserProfile other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, IsResident);
        }
    }
}