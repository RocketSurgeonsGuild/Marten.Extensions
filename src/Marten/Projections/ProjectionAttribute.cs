using System;

namespace Rocket.Surgery.Extensions.Marten.Projections
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ProjectionAttribute : Attribute
    {
        public ProjectionAttribute(ProjectionType projectionType = ProjectionType.Inline)
        {
            ProjectionType = projectionType;
        }

        public ProjectionType ProjectionType { get; set; }
    }
}
