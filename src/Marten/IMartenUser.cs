namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// IMartenUser
    /// </summary>
    public interface IMartenUser
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        object? Id { get; }
    }
}