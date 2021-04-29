using System;
using System.Collections.Generic;
using Mediaverse.Application.Authentication.Services;
using Mediaverse.Application.Common.Services;

namespace Mediaverse.Infrastructure.Authentication.Services
{
    public class NameGenerator : INameGenerator
    {
        private readonly IIdentifierProvider _identifierProvider;
        
        private readonly List<string> _nameParts = new List<string>
        {
            "squirrel",
            "dog",
            "chimpanzee",
            "lion",
            "panda",
            "rabbit"
        };

        public NameGenerator(IIdentifierProvider identifierProvider)
        {
            _identifierProvider = identifierProvider;
        }

        public string GenerateAnonymousName()
        {
            var random = new Random();
            return $"anonymous_{_nameParts[random.Next(0, _nameParts.Count - 1)]}_" + random.Next(100, 999).ToString();
            
        }

        public string GenerateAnonymousPassword()
            => _identifierProvider.GenerateToken(
                _identifierProvider.GenerateGuid());
    }
}