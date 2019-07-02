using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Rocket.Surgery.Domain;

namespace Rocket.Surgery.Extensions.Marten.Security
{
    /// <summary>
    /// CanBeAssignedSecurityQueryPart.
    /// Implements the <see cref="ISecurityQueryPart" />
    /// </summary>
    /// <seealso cref="ISecurityQueryPart" />
    class CanBeAssignedSecurityQueryPart : ISecurityQueryPart
    {
        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="constant">The constant.</param>
        /// <returns>Expression.</returns>
        public Expression GetExpression(ParameterExpression parameter, ConstantExpression constant)
        {
            var idType = constant.Type;
            var prop = Expression.Property(
                Expression.Property(parameter, nameof(ICanBeAssigned<object>.AssignedUsers)),
                nameof(AssignedUsersData<object>.UserIds)
            );

            return Expression.Call(
                prop,
                typeof(ICollection<>).MakeGenericType(idType).GetMethod(nameof(ICollection<object>.Contains), BindingFlags.Instance | BindingFlags.Public),
                constant
            );
        }

        /// <summary>
        /// Shoulds the apply.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ShouldApply(Type type)
        {
            return type.GetInterface(nameof(ICanBeAssigned<object>)+"`1") != null;
        }
    }
}
