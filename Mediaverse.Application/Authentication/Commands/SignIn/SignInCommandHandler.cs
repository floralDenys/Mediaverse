using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.Authentication.Commands.SignUp;
using Mediaverse.Application.Authentication.Common.Dtos;
using Mediaverse.Domain.Authentication.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.Authentication.Commands.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignUpCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<SignInCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SignInCommandHandler(
            IUserRepository userRepository,
            ILogger<SignInCommandHandler> logger,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<UserDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(request.Email, cancellationToken) 
                           ?? throw new ArgumentException("User with given email could not be found");

                if (user.Password != request.Password)
                {
                    throw new ArgumentException("Given password is incorrect");
                }

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not sign in {request.Email}", exception);
                throw new InvalidOperationException("Email or Password is invalid. Please retry");
            }
        }
    }
}