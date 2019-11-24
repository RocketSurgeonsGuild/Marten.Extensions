using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// ClaimsExtensions.
    /// </summary>
    public static class ClaimsExtensions
    {
        /// <summary>
        /// Gets the identifier from claims.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="options">The options.</param>
        /// <returns>System.String.</returns>
        public static string? GetIdFromClaims(this IEnumerable<Claim> claims, MartenOptions options) => claims
           .Where(options.IsIdLikeClaim)
           .FirstOrDefault()?.Value;

        private static readonly string[] IdKeys = { "user_id", "sub" };
    }
}