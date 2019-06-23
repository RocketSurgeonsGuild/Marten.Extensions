using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Services;
using NodaTime;
using Rocket.Surgery.Domain;
using Rocket.Surgery.Reflection.Extensions;

namespace Rocket.Surgery.Extensions.Marten.Listeners
{
    public class MartenDocumentSessionListener : DocumentSessionListenerBase
    {
        private readonly IMartenContext _context;
        private readonly IClock _clock;
        private static readonly BackingFieldHelper BackingFieldHelper = new BackingFieldHelper();
        private static MethodInfo HandleUnitOfWorkMethod = typeof(MartenDocumentSessionListener)
            .GetMethod(nameof(HandleUnitOfWork), BindingFlags.Instance | BindingFlags.NonPublic);
        private static ConcurrentDictionary<Type, MethodInfo> _methods = new ConcurrentDictionary<Type, MethodInfo>();
        private static MethodInfo GetMethod(Type keyType)
        {
            if (!_methods.TryGetValue(keyType, out var method))
            {
                method = HandleUnitOfWorkMethod.MakeGenericMethod(keyType);
                _methods.TryAdd(keyType, method);
            }
            return method;
        }


        public MartenDocumentSessionListener(IClock clock, IMartenContext context)
        {
            _clock = clock;
            _context = context;
        }

        public override Task BeforeSaveChangesAsync(IDocumentSession session, CancellationToken token)
        {
            BeforeSaveChanges(session);
            return Task.CompletedTask;
        }

        public override void BeforeSaveChanges(IDocumentSession session)
        {
            if (_context is null) return;
            var now = _clock.GetCurrentInstant();
            InnerHandleUnitOfWork(session.PendingChanges, now);
        }

        private void InnerHandleUnitOfWork(IUnitOfWork unitOfWork, Instant offset)
        {
            if (_context.User is null || _context.User.Id == null)
            {
                GetMethod(typeof(object)).Invoke(this, new object[] { unitOfWork, offset });
            }
            else
            {
                GetMethod(_context.User.Id.GetType()).Invoke(this, new object[] { unitOfWork, offset });
            }
        }

        private void HandleUnitOfWork<TKey>(IUnitOfWork unitOfWork, Instant offset)
        {
            var userId = default(TKey);
            if (_context.User?.Id != null)
            {
                userId = (TKey)_context.User?.Id;
            }
            foreach (var item in unitOfWork.Inserts().Concat(unitOfWork.Updates()))
            {
                if (item is IHaveOwner<TKey> hasOwner)
                {
                    Apply(hasOwner, userId);
                }
                if (item is IHaveCreatedBy<TKey> hasCreated && hasCreated.Created != null && hasCreated.Created.By.Equals(default(TKey)))
                {
                    Apply(hasCreated, userId, offset);
                }
                if (item is IHaveUpdatedBy<TKey> hasUpdated)
                {
                    Apply(hasUpdated, userId, offset);
                }
            }
        }

        public static void Apply<TKey>(IHaveOwner<TKey> document, TKey value)
        {
            BackingFieldHelper.SetBackingField(
                document,
                x => x.Owner,
                new OwnerData<TKey>(value)
            );
        }

        public static void Apply<TKey>(IHaveCreatedBy<TKey> document, TKey value, Instant offset)
        {
            BackingFieldHelper.SetBackingField(
                document,
                x => x.Created,
                new ChangeData<TKey>(value, offset)
            );
        }

        public static void Apply<TKey>(IHaveUpdatedBy<TKey> document, TKey value, Instant offset)
        {
            BackingFieldHelper.SetBackingField(
                document,
                x => x.Updated,
                new ChangeData<TKey>(value, offset)
            );
        }
    }
}
