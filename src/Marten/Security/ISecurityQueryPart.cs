using System;
using System.Linq.Expressions;

namespace Rocket.Surgery.Extensions.Marten.Security
{
    public interface ISecurityQueryPart
    {
        bool ShouldApply(Type type);
        Expression GetExpression(ParameterExpression parameter, ConstantExpression constant);
    }
}
