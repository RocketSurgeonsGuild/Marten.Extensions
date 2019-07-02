using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rocket.Surgery.Conventions.Reflection;

namespace Rocket.Surgery.Extensions.Marten.Projections
{
    /// <summary>
    /// ProjectionDescriptorCollection.
    /// </summary>
    class ProjectionDescriptorCollection
    {
        private readonly IAssemblyCandidateFinder _assemblyCandidateFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionDescriptorCollection" /> class.
        /// </summary>
        /// <param name="assemblyCandidateFinder">The assembly candidate finder.</param>
        public ProjectionDescriptorCollection(IAssemblyCandidateFinder assemblyCandidateFinder)
        {
            this._assemblyCandidateFinder = assemblyCandidateFinder;
        }

        /// <summary>
        /// Gets the projection descriptors.
        /// </summary>
        /// <returns>IEnumerable{ProjectionDescriptor}.</returns>
        public IEnumerable<ProjectionDescriptor> GetProjectionDescriptors()
        {
            return _assemblyCandidateFinder
                .GetCandidateAssemblies("Rocket.Surgery.Extensions.Marten")
                .SelectMany(x => x.DefinedTypes)
                .Where(x => x.IsClass)
                .Where(x => x.GetCustomAttribute<ProjectionAttribute>() != null)
                .Select(x => new ProjectionDescriptor (x.GetCustomAttribute<ProjectionAttribute>().ProjectionType, x.AsType()));
        }
    }
}
