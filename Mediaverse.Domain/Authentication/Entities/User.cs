using System;
using Mediaverse.Domain.Authentication.Enums;
using Mediaverse.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Mediaverse.Domain.Authentication.Entities
{
    public class User : IdentityUser<Guid>
    {
        public UserType Type { get; }
        
        private string _userName;
        public override string UserName
        {
            get => _userName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InformativeException("User name could not be blank");
                }
        
                _userName = value;
            }
        }

        private string _email;
        public override string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InformativeException("Email could not be blank");
                }
        
                _email = value;
            }
        }

        public User(UserType type)
        {
            try
            {
                Type = type;
            }
            catch (InformativeException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create user", exception);
            }
        }
        
        private User() { }
    }
}