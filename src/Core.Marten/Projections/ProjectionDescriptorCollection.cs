using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rocket.Surgery.Conventions.Reflection;

namespace Rocket.Surgery.Core.Marten.Projections
{
    class ProjectionDescriptorCollection
    {
        private readonly IAssemblyCandidateFinder assemblyCandidateFinder;

        public ProjectionDescriptorCollection(IAssemblyCandidateFinder assemblyCandidateFinder)
        {
            this.assemblyCandidateFinder = assemblyCandidateFinder;
        }

        public IEnumerable<ProjectionDescriptor> GetProjectionDescriptors()
        {
            return assemblyCandidateFinder
                .GetCandidateAssemblies("Rocket.Surgery.Core.Marten")
                .SelectMany(x => x.DefinedTypes)
                .Where(x => x.IsClass)
                .Where(x => x.GetCustomAttribute<ProjectionAttribute>() != null)
                .Select(x => new ProjectionDescriptor (x.GetCustomAttribute<ProjectionAttribute>().ProjectionType, x.AsType()));
        }
    }
}
