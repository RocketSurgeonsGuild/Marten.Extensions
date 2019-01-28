using System.Threading;
using System.Threading.Tasks;
using Marten;
using Rocket.Surgery.Azure.Functions;
using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.Marten.Functions
{
    class MartenFunctionInvocationFilter : IRocketSurgeryFunctionInvocationFilter
    {
        private readonly IDocumentSession _session;

        public MartenFunctionInvocationFilter(IDocumentSession session)
        {
            this._session = session;
        }

        public Task OnExecutedAsync(IRocketSurgeryFunctionExecutedContext context, CancellationToken cancellationToken)
        {
            return _session.SaveChangesAsync(cancellationToken);
        }

        public Task OnExecutingAsync(IRocketSurgeryFunctionExecutingContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
