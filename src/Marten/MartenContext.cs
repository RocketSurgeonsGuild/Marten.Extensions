using JetBrains.Annotations;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// MartenContext.
    /// Implements the <see cref="IMartenContext" />
    /// </summary>
    /// <seealso cref="IMartenContext" />
    [UsedImplicitly]
    internal class MartenContext : IMartenContext
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public IMartenUser? User { get; set; }
    }
}