using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System;

namespace Rocket.Surgery.Core.Marten
{
    public static class ClaimsExtensions
    {
        private static readonly string[] IdKeys = { ClaimTypes.NameIdentifier, "user_id", "sub" };
        public static string GetIdFromClaims(this IEnumerable<Claim> claims)
        {
            return IdKeys
                .Select(key => claims.FirstOrDefault(c => c.Type == key))
                .FirstOrDefault()?.Value;
        }
    }
}
