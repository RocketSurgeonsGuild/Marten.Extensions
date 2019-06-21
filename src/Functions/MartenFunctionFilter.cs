using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.Azure.WebJobs.Host;
#pragma warning disable 618

namespace Rocket.Surgery.Extensions.Marten.Functions
{
    class MartenFunctionFilter : IFunctionInvocationFilter, IFunctionExceptionFilter
    {
        private readonly IDocumentSession _documentSession;

        public MartenFunctionFilter(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            return _documentSession.SaveChangesAsync(cancellationToken);
        }

        public Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
