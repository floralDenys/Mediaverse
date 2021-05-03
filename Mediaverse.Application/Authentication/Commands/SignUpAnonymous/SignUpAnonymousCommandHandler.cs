using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using MediatR;
using Mediaverse.Application.Authentication.Common.Dtos;
using Mediaverse.Application.Authentication.Services;
using Mediaverse.Application.Common.Services;
using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.Authentication.Enums;
using Mediaverse.Domain.Authentication.Repositories;
using Mediaverse.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.Authentication.Commands.SignUpAnonymous
{
    public class SignUpAnonymousCommandHandler : IRequestHandler<SignUpAnonymousCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly INameGenerator _nameGenerator;
        private readonly ILogger<SignUpAnonymousCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SignUpAnonymousCommandHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            INameGenerator nameGenerator,
            ILogger<SignUpAnonymousCommandHandler> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _nameGenerator = nameGenerator;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(SignUpAnonymousCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                string generatedPassword = _nameGenerator.GenerateAnonymousPassword();
                var user = new User(UserType.Anonymous) {UserName = _nameGenerator.GenerateAnonymousName()};

                var result = await _userManager.CreateAsync(user, generatedPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                else
                {
                    var errorMessages = result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException(string.Join("\n", errorMessages));
                }
                
                transaction.Complete();
                
                return Unit.Value;
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