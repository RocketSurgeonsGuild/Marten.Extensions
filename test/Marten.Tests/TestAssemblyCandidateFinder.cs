using System.Collections.Generic;
using System.Reflection;
using Rocket.Surgery.Conventions.Reflection;
//using Rocket.Surgery.Extensions.Marten.AspNetCore;

namespace Rocket.Surgery.Extensions.Marten.Tests
{
    class TestAssemblyCandidateFinder : IAssemblyCandidateFinder
    {
        public IEnumerable<Assembly> GetCandidateAssemblies(IEnumerable<string> candidates)
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
