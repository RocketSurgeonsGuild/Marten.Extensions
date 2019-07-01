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
    /// <summary>
    ///  MartenDocumentSessionListener.
    /// Implements the <see cref="Marten.DocumentSessionListenerBase" />
    /// </summary>
    /// <seealso cref="Marten.DocumentSessionListenerBase" />
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


        /// <summary>
        /// Initializes a new instance of the <see cref="MartenDocumentSessionListener"/> class.
        /// </summary>
        /// <param name="clock">The clock.</param>
        /// <param name="context">The context.</param>
        public MartenDocumentSessionListener(IClock clock, IMartenContext context)
        {
            _clock = clock;
            _context = context;
        }

        /// <summary>
        /// Befores the save changes asynchronous.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        public override Task BeforeSaveChangesAsync(IDocumentSession session, CancellationToken token)
        {
            BeforeSaveChanges(session);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Befores the save changes.
        /// </summary>
        /// <param name="session">The session.</param>
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

        /// <summary>
        /// Applies the specified document.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="document">The document.</param>
        /// <param name="value">The value.</param>
        public static void Apply<TKey>(IHaveOwner<TKey> document, TKey value)
        {
            BackingFieldHelper.SetBackingField(
                document,
                x => x.Owner,
                new OwnerData<TKey>(value)
            );
        }

        /// <summary>
        /// Applies the specified document.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="document">The document.</param>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        public static void Apply<TKey>(IHaveCreatedBy<TKey> document, TKey value, Instant offset)
        {
            BackingFieldHelper.SetBackingField(
                document,
                x => x.Created,
                new ChangeData<TKey>(value, offset)
            );
        }

        /// <summary>
        /// Applies the specified document.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="document">The document.</param>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
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
