using System;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Invitation
    {
        public string Token { get; }

        public Invitation(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException(nameof(token));
                }

                Token = token;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create invitation", exception);
            }
        }
    }
}