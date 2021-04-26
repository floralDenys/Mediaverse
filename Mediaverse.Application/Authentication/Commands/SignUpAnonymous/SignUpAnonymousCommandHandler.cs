using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.Authentication.Common.Dtos;
using Mediaverse.Application.Authentication.Services;
using Mediaverse.Application.Common.Services;
using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.Authentication.Enums;
using Mediaverse.Domain.Authentication.Repositories;
using Mediaverse.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.Authentication.Commands.SignUpAnonymous
{
    public class SignUpAnonymousCommandHandler : IRequestHandler<SignUpAnonymousCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentifierProvider _identifierProvider;
        private readonly INameGenerator _nameGenerator;
        private readonly ILogger<SignUpAnonymousCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SignUpAnonymousCommandHandler(
            IUserRepository userRepository,
            IIdentifierProvider identifierProvider,
            INameGenerator nameGenerator,
            ILogger<SignUpAnonymousCommandHandler> logger,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _identifierProvider = identifierProvider;
            _nameGenerator = nameGenerator;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<UserDto> Handle(SignUpAnonymousCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid userId = _identifierProvider.GenerateGuid();
                string generatedNickname = _nameGenerator.GenerateAnonymousName();

                var user = new User(userId, UserType.Anonymous)
                {
                    Nickname = generatedNickname,
                    LastActive = DateTime.Now
                };

                await _userRepository.AddUserAsync(user, cancellationToken);

                return _mapper.Map<UserDto>(user);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, "Could not sign up as anonymous");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not sign up as anonymous");
                throw new InformativeException("Could not sign up as anonymous. Please retry");
            }
        }
    }
}