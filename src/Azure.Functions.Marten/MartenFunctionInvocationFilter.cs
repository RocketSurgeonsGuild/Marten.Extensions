using System;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.Azure.WebJobs.Host;

namespace Rocket.Surgery.Azure.Functions.Marten
{
    public class MartenFunctionInvocationFilter : IFunctionInvocationFilter
    {
        private readonly IDocumentSession _documentSession;

        public MartenFunctionInvocationFilter(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            await _documentSession.SaveChangesAsync(cancellationToken);
        }
    }
}
