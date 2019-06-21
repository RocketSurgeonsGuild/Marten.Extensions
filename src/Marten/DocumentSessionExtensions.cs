using Marten;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Rocket.Surgery.Extensions.Marten.Security;
using Rocket.Surgery.Domain;

namespace Rocket.Surgery.Extensions.Marten
{
    public static class DocumentSessionExtensions
    {
        public static IDocumentSession RegisterListeners(this IDocumentSession documentSession, IServiceProvider serviceProvider)
        {
            foreach (var listener in serviceProvider.GetServices<IDocumentSessionListener>())
                documentSession.Listeners.Add(listener);
            return documentSession;
        }

        internal static IDocumentSession RegisterListeners(this IDocumentSession documentSession, IEnumerable<IDocumentSessionListener> documentSessionListeners)
        {
            foreach (var listener in documentSessionListeners)
                documentSession.Listeners.Add(listener);
            return documentSession;
        }

        public static IQueryable<T> OnlyItemsTheUserCanSee<T>(this IQueryable<T> query, ISecurityQueryProvider securityQueryProvider, IMartenContext context)
        {
            if (context.User?.Id != null)
            {
                var expression = securityQueryProvider.GetExpression<T>(context.User.Id);
                if (expression != null)
                    return query.Where(expression);
            }
            return query;
        }

        public static IEnumerable<T> OnlyItemsTheUserCanSee<T>(this IEnumerable<T> results, ISecurityQueryProvider securityQueryProvider, IMartenContext  context)
        {
            if (context.User?.Id != null)
            {
                var expression = securityQueryProvider.GetExpression<T>(context.User.Id);
                if (expression != null)
                    return results.Where(expression.Compile());
            }
            return results;
        }
    }
}
