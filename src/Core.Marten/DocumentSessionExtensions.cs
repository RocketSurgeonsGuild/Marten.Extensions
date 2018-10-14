using Marten;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Rocket.Surgery.Core.Marten.Security;
using Rocket.Surgery.Domain;

namespace Rocket.Surgery.Core.Marten
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

        public static IQueryable<T> OnlyItemsTheUserCanSee<T>(this IQueryable<T> query, ISecurityQueryProvider securityQueryProvider, IMartenUser user)
        {
            if (user?.Id != null)
            {
                var expression = securityQueryProvider.GetExpression<T>(user.Id);
                if (expression != null)
                    return query.Where(expression);
            }
            return query;
        }

        public static IEnumerable<T> OnlyItemsTheUserCanSee<T>(this IEnumerable<T> results, ISecurityQueryProvider securityQueryProvider, IMartenUser user)
        {
            if (user?.Id != null)
            {
                var expression = securityQueryProvider.GetExpression<T>(user.Id);
                if (expression != null)
                    return results.Where(expression.Compile());
            }
            return results;
        }
    }
}
