using MediatR;
using Mediaverse.Application.Authentication.Common.Dtos;

namespace Mediaverse.Application.Authentication.Commands.SignIn
{
    public class SignInCommand : IRequest<UserDto>
    {
        public string LoginOrEmail { get; set; }
        public string Password { get; set; }
    }
}