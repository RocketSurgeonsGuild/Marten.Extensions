namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// IMartenTenant
    /// </summary>
    public interface IMartenTenant
    {
        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>The tenant identifier.</value>
        string TenantId { get; set; }
    }
}