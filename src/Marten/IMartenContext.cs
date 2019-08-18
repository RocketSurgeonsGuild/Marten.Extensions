namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// IMartenContext
    /// </summary>
    public interface IMartenContext
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        IMartenUser? User { get; set; }
    }
}
