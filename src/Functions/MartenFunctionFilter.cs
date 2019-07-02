using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Marten;
using Microsoft.Azure.WebJobs.Host;
#pragma warning disable 618

namespace Rocket.Surgery.Extensions.Marten.Functions
{
    /// <summary>
    /// MartenFunctionFilter.
    /// Implements the <see cref="IFunctionInvocationFilter" />
    /// Implements the <see cref="IFunctionExceptionFilter" />
    /// </summary>
    /// <seealso cref="IFunctionInvocationFilter" />
    /// <seealso cref="IFunctionExceptionFilter" />
    [UsedImplicitly]
    class MartenFunctionFilter : IFunctionInvocationFilter, IFunctionExceptionFilter
    {
        private readonly IDocumentSession _documentSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenFunctionFilter" /> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public MartenFunctionFilter(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        /// <summary>
        /// Called when [executing asynchronous].
        /// </summary>
        /// <param name="executingContext">The executing context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when [executed asynchronous].
        /// </summary>
        /// <param name="executedContext">The executed context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            return _documentSession.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Called when [exception asynchronous].
        /// </summary>
        /// <param name="exceptionContext">The exception context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
