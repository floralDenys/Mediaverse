using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MediatR;
using Mediaverse.Application.Authentication.Services;
using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.Authentication.Commands.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        
        private readonly IEmailService _emailService;
        private readonly ILogger<SignInCommandHandler> _logger;

        public SignInCommandHandler(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IEmailService emailService,
            ILogger<SignInCommandHandler> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                if (string.IsNullOrEmpty(request.LoginOrEmail)
                    || string.IsNullOrEmpty(request.Password))
                {
                    throw new InformativeException("Please provide valid login/email and password");
                }
                
                string login = request.LoginOrEmail;
                if (_emailService.IsValidEmail(request.LoginOrEmail))
                {
                    var user = await _userManager.FindByEmailAsync(request.LoginOrEmail);
                    login = user?.UserName ?? login;
                }

                var result = await _signInManager.PasswordSignInAsync(
                    login,
                    request.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    throw new InformativeException("Email or password is incorrect");
                }
                
                transaction.Complete();
                
                return Unit.Value;
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not sign in {request.LoginOrEmail}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not sign in {request.LoginOrEmail}");
                throw new InformativeException("Something went wrong. Please retry");
            }
        }
    }
}