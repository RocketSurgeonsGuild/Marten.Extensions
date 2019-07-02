using Marten;
using Marten.Services;
using Rocket.Surgery.Extensions.Marten.Security;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// SecureQuerySessionExtensions.
    /// </summary>
    public static class SecureQuerySessionExtensions
    {
        /// <summary>
        /// Secures the query session.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="securityQueryProvider">The security query provider.</param>
        /// <param name="martenContext">The marten context.</param>
        /// <returns>ISecureQuerySession.</returns>
        public static ISecureQuerySession SecureQuerySession(this IDocumentStore store, ISecurityQueryProvider securityQueryProvider, IMartenContext martenContext)
        {
            return new SecureQuerySession(store.QuerySession(), securityQueryProvider, martenContext);
        }

        /// <summary>
        /// Secures the query session.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="securityQueryProvider">The security query provider.</param>
        /// <param name="martenContext">The marten context.</param>
        /// <returns>ISecureQuerySession.</returns>
        public static ISecureQuerySession SecureQuerySession(this IDocumentStore store, string tenantId, ISecurityQueryProvider securityQueryProvider, IMartenContext martenContext)
        {
            return new SecureQuerySession(store.QuerySession(tenantId), securityQueryProvider, martenContext);
        }

        /// <summary>
        /// Secures the query session.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="options">The options.</param>
        /// <param name="securityQueryProvider">The security query provider.</param>
        /// <param name="martenContext">The marten context.</param>
        /// <returns>ISecureQuerySession.</returns>
        public static ISecureQuerySession SecureQuerySession(this IDocumentStore store, SessionOptions options, ISecurityQueryProvider securityQueryProvider, IMartenContext martenContext)
        {
            return new SecureQuerySession(store.QuerySession(options), securityQueryProvider, martenContext);
        }
    }
}
