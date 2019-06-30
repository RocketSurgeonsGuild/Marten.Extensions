using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Rocket.Surgery.Domain;

namespace Rocket.Surgery.Extensions.Marten.Security
{
    /// <summary>
    /// Class HaveOwnerSecurityQueryPart.
    /// Implements the <see cref="Rocket.Surgery.Extensions.Marten.Security.ISecurityQueryPart" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.Marten.Security.ISecurityQueryPart" />
    public class HaveOwnerSecurityQueryPart : ISecurityQueryPart
    {
        public Expression GetExpression(ParameterExpression parameter, ConstantExpression constant)
        {
            var idType = constant.Type;
            var prop = Expression.Property(
                Expression.Property(parameter, nameof(IHaveOwner<object>.Owner)),
                nameof(OwnerData<object>.Id)
            );

            var methods = idType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetParameters().Length == 1)
                .Where(x => x.Name == nameof(object.Equals))
                .OrderByDescending(x => x.GetParameters()[0].ParameterType == idType)
                .ToArray() ;

            return Expression.Call(
                prop,
                methods.First(),
                constant
            );
        }

        public bool ShouldApply(Type type)
        {
            return type.GetInterface(nameof(IHaveOwner<object>)+"`1") != null;
        }
    }
}
