using System.Collections.Generic;
using System.Reflection;
using Rocket.Surgery.Extensions.Marten.AspNetCore;
using Rocket.Surgery.Extensions.Marten;
using Rocket.Surgery.Conventions.Reflection;

namespace Rocket.Surgery.Marten.Tests
{
    class TestAssemblyCandidateFinder : IAssemblyCandidateFinder
    {
        public IEnumerable<Assembly> GetCandidateAssemblies(IEnumerable<string> candidates)
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
