namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// Interface IMartenContext
    /// </summary>
    public interface IMartenContext
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        IMartenUser User { get; set; }
    }
    /// <summary>
    /// Class MartenContext.
    /// Implements the <see cref="Rocket.Surgery.Extensions.Marten.IMartenContext" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.Marten.IMartenContext" />
    class MartenContext : IMartenContext
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public IMartenUser User { get; set; }
    }
}
