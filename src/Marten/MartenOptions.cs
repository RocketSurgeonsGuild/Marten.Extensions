using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Marten;
using Marten.Services;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Rocket.Surgery.Extensions.Marten
{
    public class MartenOptions
    {
        public string[] IdClaims { get; set; } = Array.Empty<string>();

        public bool IsIdLikeClaim(Claim claim)
        {
            if (IdClaims != null && IdClaims.Length > 0 && IdClaims.Any(z => claim.Type == z))
            {
                return true;
            }

            if (claim.Type == ClaimTypes.NameIdentifier)
            {
                return true;
            }

            if ((claim.Type.StartsWith("http://") || claim.Type.StartsWith("https://"))
            && (claim.Type.EndsWith("/sub") || claim.Type.EndsWith("/id") || claim.Type.EndsWith("/user_id")))
            {
                return true;
            }

            return claim.Type == "user_id" || claim.Type == "sub";
        }
    }
}
