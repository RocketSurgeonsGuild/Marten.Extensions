using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    ///  ClaimsExtensions.
    /// </summary>
    public static class ClaimsExtensions
    {
        private static readonly string[] IdKeys = { "user_id", "sub" };
        /// <summary>
        /// Gets the identifier from claims.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="options">The options.</param>
        /// <returns>System.String.</returns>
        public static string GetIdFromClaims(this IEnumerable<Claim> claims, MartenOptions options)
        {
            return claims
                .Where(options.IsIdLikeClaim)
                .FirstOrDefault()?.Value;
        }
    }
}
