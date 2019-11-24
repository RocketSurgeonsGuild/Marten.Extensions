using System;

namespace Rocket.Surgery.Extensions.Marten.Projections
{
    /// <summary>
    /// ProjectionDescriptor.
    /// </summary>
    internal class ProjectionDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionDescriptor" /> class.
        /// </summary>
        /// <param name="projectionType">Type of the projection.</param>
        /// <param name="type">The type.</param>
        public ProjectionDescriptor(ProjectionType projectionType, Type type)
        {
            ProjectionType = projectionType;
            Type = type;
        }

        /// <summary>
        /// Gets the type of the projection.
        /// </summary>
        /// <value>The type of the projection.</value>
        public ProjectionType ProjectionType { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; }
    }
}