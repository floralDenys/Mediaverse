using System;
using System.Security.Claims;

namespace Mediaverse.Infrastructure.Authentication.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetCurrentUserId(this ClaimsPrincipal principal) =>
            Guid.Parse((ReadOnlySpan<char>) principal.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}