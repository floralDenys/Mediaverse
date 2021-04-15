namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Invitation
    {
        public string Token { get; }
        public string Password { get; }

        public Invitation(
            string token,
            string password)
        {
            Token = token;
            Password = password;
        }
    }
}