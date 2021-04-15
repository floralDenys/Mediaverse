namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Invitation
    {
        public string Token { get; }

        public Invitation(string token)
        {
            Token = token;
        }
    }
}