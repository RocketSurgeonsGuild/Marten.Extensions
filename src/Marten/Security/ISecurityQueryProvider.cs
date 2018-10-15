using System;
using System.Linq.Expressions;

namespace Rocket.Surgery.Extensions.Marten.Security
{
    public interface ISecurityQueryProvider
    {
        Expression<Func<T, bool>> GetExpression<T>(object userId);
    }
}
