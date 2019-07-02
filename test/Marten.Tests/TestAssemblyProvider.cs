using System.Collections.Generic;
using System.Reflection;
using Rocket.Surgery.Conventions.Reflection;
//using Rocket.Surgery.Extensions.Marten.AspNetCore;

namespace Rocket.Surgery.Extensions.Marten.Tests
{
    class TestAssemblyProvider : IAssemblyProvider
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            return new[]
            {
                typeof(TestAssemblyProvider).GetTypeInfo().Assembly,
                typeof(DocumentSessionExtensions).GetTypeInfo().Assembly,
                //typeof(MartenMiddleware).GetTypeInfo().Assembly,
            };
        }
    }
}
