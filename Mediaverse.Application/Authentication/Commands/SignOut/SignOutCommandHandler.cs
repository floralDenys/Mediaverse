using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.Authentication.Commands.SignOut
{
    public class SignOutCommandHandler : IRequestHandler<SignOutCommand>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<SignOutCommandHandler> _logger;

        public SignOutCommandHandler(
            SignInManager<User> signInManager,
            ILogger<SignOutCommandHandler> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _signInManager.SignOutAsync();
                
                return Unit.Value;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not log out user");
                throw new InformativeException("Log Out attempt failed. Please retry.");
            }
        }
    }
}