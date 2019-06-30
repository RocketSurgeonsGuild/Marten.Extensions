using System;
using System.Linq.Expressions;

namespace Rocket.Surgery.Extensions.Marten.Security
{
    /// <summary>
    /// Interface ISecurityQueryPart
    /// </summary>
    public interface ISecurityQueryPart
    {
        /// <summary>
        /// Shoulds the apply.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ShouldApply(Type type);
        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="constant">The constant.</param>
        /// <returns>Expression.</returns>
        Expression GetExpression(ParameterExpression parameter, ConstantExpression constant);
    }
}
