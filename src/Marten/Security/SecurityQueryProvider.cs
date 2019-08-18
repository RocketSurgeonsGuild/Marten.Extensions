using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;

namespace Rocket.Surgery.Extensions.Marten.Security
{
    /// <summary>
    /// SecurityQueryProvider.
    /// Implements the <see cref="ISecurityQueryProvider" />
    /// </summary>
    /// <seealso cref="ISecurityQueryProvider" />
    class SecurityQueryProvider : ISecurityQueryProvider
    {
        private readonly IEnumerable<ISecurityQueryPart> _parts;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityQueryProvider" /> class.
        /// </summary>
        /// <param name="parts">The parts.</param>
        public SecurityQueryProvider(IEnumerable<ISecurityQueryPart> parts)
        {
            _parts = parts;
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Expression{Func{T, System.Boolean}}.</returns>
        public Expression<Func<T, bool>>? GetExpression<T>(object userId)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var constantUserId = Expression.Constant(userId, userId.GetType());

            var expressions = _parts
                .Where(x => x.ShouldApply(typeof(T)))
                .Select(x => x.GetExpression(param, constantUserId))
                .ToArray();
            if (!expressions.Any())
            {
                return null;
            }

            Expression? e = null;
            foreach (var exp in expressions)
            {
                if (e == null)
                {
                    e = exp;
                }
                else
                {
                    e = Expression.OrElse(e, exp);
                }
            }

            return Expression.Lambda<Func<T, bool>>(e, param);
        }
    }
}
