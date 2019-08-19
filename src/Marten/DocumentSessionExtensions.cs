using Marten;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// DocumentSessionExtensions.
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
    }
}
