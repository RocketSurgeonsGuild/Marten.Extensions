using System;

namespace Rocket.Surgery.Extensions.Marten.Builders
{
    /// <summary>
    /// Class MartenConfigurationDelegateContainer.
    /// </summary>
    class MartenConfigurationDelegateContainer
    {
        /// <summary>
        /// Gets the delegate.
        /// </summary>
        /// <value>The delegate.</value>
        public Delegate Delegate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenConfigurationDelegateContainer"/> class.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        public MartenConfigurationDelegateContainer(Delegate @delegate)
        {
            Delegate = @delegate;
        }
    }
}
