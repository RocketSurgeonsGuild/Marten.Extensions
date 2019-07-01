using System;
using System.Linq.Expressions;

namespace Rocket.Surgery.Extensions.Marten.Security
{
    /// <summary>
    ///  ISecurityQueryProvider
    /// </summary>
    public interface ISecurityQueryProvider
    {
        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        Expression<Func<T, bool>> GetExpression<T>(object userId);
    }
}
