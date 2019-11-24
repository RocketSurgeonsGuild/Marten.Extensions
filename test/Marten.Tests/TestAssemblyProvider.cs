using System.Collections.Generic;
using System.Reflection;
using Rocket.Surgery.Conventions.Reflection;

//using Rocket.Surgery.Extensions.Marten.AspNetCore;

namespace Rocket.Surgery.Extensions.Marten.Tests
{
    internal class TestAssemblyProvider : IAssemblyProvider
    {
        public IEnumerable<Assembly> GetAssemblies() => new[]
        {
            typeof(TestAssemblyProvider).GetTypeInfo().Assembly,
            typeof(DocumentSessionExtensions).GetTypeInfo().Assembly
            //typeof(MartenMiddleware).GetTypeInfo().Assembly,
        };
    }
}