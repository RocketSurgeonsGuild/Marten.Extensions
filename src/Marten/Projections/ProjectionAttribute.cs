using System;

namespace Rocket.Surgery.Extensions.Marten.Projections
{
    /// <summary>
    ///  ProjectionAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ProjectionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionAttribute"/> class.
        /// </summary>
        /// <param name="projectionType">Type of the projection.</param>
        public ProjectionAttribute(ProjectionType projectionType = ProjectionType.Inline)
        {
            ProjectionType = projectionType;
        }

        /// <summary>
        /// Gets or sets the type of the projection.
        /// </summary>
        /// <value>The type of the projection.</value>
        public ProjectionType ProjectionType { get; set; }
    }
}
