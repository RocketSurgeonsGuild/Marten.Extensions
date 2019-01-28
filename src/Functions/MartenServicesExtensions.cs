using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rocket.Surgery.Extensions.Marten.AspNetCore;
using Rocket.Surgery.Extensions.Marten;
using Rocket.Surgery.Extensions.Marten.Builders;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using Rocket.Surgery.Azure.Functions;
using Rocket.Surgery.Extensions.Marten.Functions;

namespace Rocket.Surgery.Extensions.Marten.Functions
{
    public static class MartenServicesExtensions
    {
        public static MartenServicesBuilder AddFunctionFilters(this MartenServicesBuilder builder)
        {
            builder.Parent.Services.TryAddEnumerable(ServiceDescriptor.Transient<IRocketSurgeryFunctionInvocationFilter, MartenFunctionInvocationFilter>());
            return builder;
        }
    }
}
