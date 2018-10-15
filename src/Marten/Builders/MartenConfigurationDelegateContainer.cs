using System;

namespace Rocket.Surgery.Extensions.Marten.Builders
{
    class MartenConfigurationDelegateContainer
    {
        public Delegate Delegate { get; }

        public MartenConfigurationDelegateContainer(Delegate @delegate)
        {
            Delegate = @delegate;
        }
    }
}