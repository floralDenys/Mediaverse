using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.Authentication.Common.Dtos;
using Mediaverse.Application.Authentication.Services;
using Mediaverse.Domain.Authentication.Repositories;
using Mediaverse.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.Authentication.Commands.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;

        private readonly IEmailService _emailService;
        private readonly ILogger<SignInCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SignInCommandHandler(
            IUserRepository userRepository,
            IEmailService emailService,
            ILogger<SignInCommandHandler> logger,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<UserDto> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _emailService.IsValidEmail(request.LoginOrEmail)
                    ? await _userRepository.GetUserByEmailAsync(request.LoginOrEmail, cancellationToken)
                    : await _userRepository.GetUserByLoginAsync(request.LoginOrEmail, cancellationToken);

                _ = user ?? throw new ArgumentException("User with given email could not be found");

                if (user.Password != request.Password)
                {
                    throw new ArgumentException("Given password is incorrect");
                }

                return _mapper.Map<UserDto>(user);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not sign in {request.LoginOrEmail}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not sign in {request.LoginOrEmail}");
                throw new InformativeException("Email or Password is invalid. Please retry");
            }
        }
    }
}