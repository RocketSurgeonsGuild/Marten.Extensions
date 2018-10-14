using System;

namespace Rocket.Surgery.Core.Marten.Projections
{
    class ProjectionDescriptor
    {
        public ProjectionDescriptor(ProjectionType projectionType, Type type)
        {
            ProjectionType = projectionType;
            Type = type;
        }

        public ProjectionType ProjectionType { get; }
        public Type Type { get; }
    }
}
