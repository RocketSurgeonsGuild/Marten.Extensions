using System;
using System.Linq;
using System.Linq.Expressions;
using Marten.Linq;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace Rocket.Surgery.Core.Marten
{
    // public class SecureMartenQueryProvider : MartenQueryProvider
    // {
    //     private Expression _whereExpression;

    //     public SecureMartenQueryProvider(Expression whereExpression, Type queryableType, IQueryParser queryParser, IQueryExecutor executor) : base(queryableType, queryParser, executor)
    //     {
    //         _whereExpression = whereExpression;
    //     }

    //     public override IQueryable<T> CreateQuery<T>(Expression expression)
    //     {
    //         var query = base.CreateQuery<T>(expression);
    //         if (_whereExpression != null) {
    //             var exp = _whereExpression;
    //             _whereExpression = null;
    //             return query.Where((Expression<Func<T, bool>>)exp);
    //         }
    //         return query;
    //     }
    // }
}
