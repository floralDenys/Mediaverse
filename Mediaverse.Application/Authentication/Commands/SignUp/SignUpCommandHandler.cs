using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.Authentication.Common.Dtos;
using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.Authentication.Enums;
using Mediaverse.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.Authentication.Commands.SignUp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, UserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<SignUpCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SignUpCommandHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<SignUpCommandHandler> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<UserDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email)
                    || string.IsNullOrEmpty(request.Login)
                    || string.IsNullOrEmpty(request.Password)
                    || string.IsNullOrEmpty(request.PasswordConfirmation))
                {
                    throw new InformativeException("Please provide valid email, login and " +
                                                   "password with confirmation");
                }
                
                if (request.Password != request.PasswordConfirmation)
                {
                    throw new InformativeException("Password and Confirmation does not match");
                }

                var user = new User(UserType.Member)
                {
                    UserName = request.Login,
                    Email = request.Email,
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                else
                {
                    var errorMessages = result.Errors.Select(e => e.Description);
                    throw new InformativeException(string.Join("\n", errorMessages));
                }

                return _mapper.Map<UserDto>(user);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not sign up user {request.Email}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not sign up user {request.Email}");
                throw new InformativeException("Could not sign up user. Please retry");
            }
        }
    }
}