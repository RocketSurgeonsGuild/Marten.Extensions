using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Rocket.Surgery.Domain;

namespace Rocket.Surgery.Core.Marten.Security
{
    public class CanBeAssignedSecurityQueryPart : ISecurityQueryPart
    {
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

        public bool ShouldApply(Type type)
        {
            return type.GetInterface(nameof(ICanBeAssigned<object>)+"`1") != null;
        }
    }
}
