using System;
using Mediaverse.Domain.Common;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class UserProfile : Entity
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
    }
}