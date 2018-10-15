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
        private readonly IMartenUser _martenUser;
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


        public MartenDocumentSessionListener(IClock clock, IMartenUser martenUser = null)
        {
            _clock = clock;
            _martenUser = martenUser;
        }

        public override Task BeforeSaveChangesAsync(IDocumentSession session, CancellationToken token)
        {
            BeforeSaveChanges(session);
            return Task.CompletedTask;
        }

        public override void BeforeSaveChanges(IDocumentSession session)
        {
            if (_martenUser is null) return;
            var now = _clock.GetCurrentInstant().ToDateTimeOffset();
            InnerHandleUnitOfWork(session.PendingChanges, now);
        }

        private void InnerHandleUnitOfWork(IUnitOfWork unitOfWork, DateTimeOffset offset) {
            if (_martenUser is null) return;
            GetMethod(_martenUser.Id.GetType()).Invoke(this, new object[] { unitOfWork, offset });
        }

        private void HandleUnitOfWork<TKey>(IUnitOfWork unitOfWork, DateTimeOffset offset)
        {
            foreach (var item in unitOfWork.Inserts().Concat(unitOfWork.Updates()))
            {
                if (item is IHaveOwner<TKey> hasOwner)
                {
                    Apply(hasOwner);
                }
                if (item is IHaveCreatedBy<TKey> hasCreated && hasCreated.Created != null && hasCreated.Created.By.Equals(default(TKey)))
                {
                    Apply(hasCreated, offset);
                }
                if (item is IHaveUpdatedBy<TKey> hasUpdated)
                {
                    Apply(hasUpdated, offset);
                }
            }
        }

        public void Apply<TKey>(IHaveOwner<TKey> document)
        {
            BackingFieldHelper.SetBackingField(
                document,
                x => x.Owner,
                new OwnerData<TKey>((TKey)_martenUser.Id)
            );
        }

        public void Apply<TKey>(IHaveCreatedBy<TKey> document, DateTimeOffset offset)
        {
            BackingFieldHelper.SetBackingField(
                document,
                x => x.Created,
                new ChangeData<TKey>((TKey)_martenUser.Id, offset)
            );
        }

        public void Apply<TKey>(IHaveUpdatedBy<TKey> document, DateTimeOffset offset)
        {
            BackingFieldHelper.SetBackingField(
                document,
                x => x.Updated,
                new ChangeData<TKey>((TKey)_martenUser.Id, offset)
            );
        }
    }
}
