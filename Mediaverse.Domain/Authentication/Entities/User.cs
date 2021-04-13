using System;
using Mediaverse.Domain.Authentication.Enums;

namespace Mediaverse.Domain.Authentication.Entities
{
    public class User
    {
        public Guid Id { get; }

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Nickname could not be null or empty");
                }

                _nickname = value;
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Password could not be null or empty");
                }

                _password = value;
            }
        }
        
        public UserType Type { get; }

        private DateTime _lastActive;
        public DateTime LastActive
        {
            get => _lastActive;
            set
            {
                if (value == default)
                {
                    throw new ArgumentException("Given last active time is invalid");
                }

                _lastActive = value;
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Email could not be null or empty");
                }

                _email = value;
            }
        }

        public User(Guid id, UserType type)
        {
            try
            {
                if (id == default)
                {
                    throw new ArgumentException("Given ID is invalid");
                }

                Id = id;
                Type = type;
                
                _lastActive = DateTime.Now;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create user", exception);
            }
        }
        
        private User() { }
    }
}