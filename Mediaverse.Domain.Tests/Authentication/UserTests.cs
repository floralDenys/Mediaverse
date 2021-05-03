using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.Authentication.Enums;
using Xunit;

namespace Mediaverse.Domain.Tests.Authentication
{
    public class UserTests
    {
        [Theory]
        [InlineData(UserType.Member, "someusername", "someemail@test.com")]
        [InlineData(UserType.Anonymous, "someotherusername", "someotheremail@test.com")]
        public void Create_user(
            UserType actualUserType,
            string actualUserName,
            string actualEmail)
        {
            var user = new User(actualUserType)
            {
                UserName = actualUserName,
                Email = actualEmail
            };
            
            Assert.Equal(actualUserType, user.Type);
            Assert.Equal(actualUserName, user.UserName);
            Assert.Equal(actualEmail, user.Email);
        }
    }
}