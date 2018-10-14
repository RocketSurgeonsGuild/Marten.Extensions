using System.Collections.Generic;
using System.Reflection;
using Rocket.Surgery.AspNetCore.Marten;
using Rocket.Surgery.Core.Marten;
using Rocket.Surgery.Conventions.Reflection;

namespace Rocket.Surgery.Marten.Tests
{
    class TestAssemblyProvider : IAssemblyProvider
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            return new[]
            {
                typeof(TestAssemblyProvider).GetTypeInfo().Assembly,
                typeof(DocumentSessionExtensions).GetTypeInfo().Assembly,
                typeof(MartenMiddleware).GetTypeInfo().Assembly,
            };
        }
    }
}
