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
    /// <summary>
    ///  DocumentSessionExtensions.
    /// </summary>
    public static class DocumentSessionExtensions
    {
        /// <summary>
        /// Registers the listeners.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>IDocumentSession.</returns>
        public static IDocumentSession RegisterListeners(this IDocumentSession documentSession, IServiceProvider serviceProvider)
        {
            foreach (var listener in serviceProvider.GetServices<IDocumentSessionListener>())
                documentSession.Listeners.Add(listener);
            return documentSession;
        }

        /// <summary>
        /// Registers the listeners.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="documentSessionListeners">The document session listeners.</param>
        /// <returns>IDocumentSession.</returns>
        internal static IDocumentSession RegisterListeners(this IDocumentSession documentSession, IEnumerable<IDocumentSessionListener> documentSessionListeners)
        {
            foreach (var listener in documentSessionListeners)
                documentSession.Listeners.Add(listener);
            return documentSession;
        }

        /// <summary>
        /// Called when [items the user can see].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="securityQueryProvider">The security query provider.</param>
        /// <param name="context">The context.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
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

        /// <summary>
        /// Called when [items the user can see].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results">The results.</param>
        /// <param name="securityQueryProvider">The security query provider.</param>
        /// <param name="context">The context.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
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
