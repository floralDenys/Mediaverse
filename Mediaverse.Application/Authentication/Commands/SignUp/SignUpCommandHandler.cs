using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.Authentication.Common.Dtos;
using Mediaverse.Application.Common.Services;
using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.Authentication.Enums;
using Mediaverse.Domain.Authentication.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.Authentication.Commands.SignUp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentifierProvider _identifierProvider;
        private readonly ILogger<SignUpCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SignUpCommandHandler(
            IUserRepository userRepository,
            IIdentifierProvider identifierProvider,
            ILogger<SignUpCommandHandler> logger,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _identifierProvider = identifierProvider;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<UserDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(request.Email, cancellationToken);
                if (user != null)
                {
                    throw new ArgumentException("User with given email exists already");
                }

                if (request.Password != request.PasswordConfirmation)
                {
                    throw new InvalidOperationException("Password and Confirmation does not match");
                }
                
                Guid userId = _identifierProvider.GenerateGuid();
                user = new User(userId, UserType.Member)
                {
                    Nickname = request.Nickname,
                    Email = request.Email,
                    Password = request.Password
                };

                await _userRepository.AddUserAsync(user, cancellationToken);

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not sign up user {request.Email}", exception);
                throw new InvalidOperationException("Could not sign up user. Please retry");
            }
        }
    }
}