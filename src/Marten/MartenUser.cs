using System;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// MartenUser.
    /// Implements the <see cref="IMartenUser" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IMartenUser" />
    public class MartenUser<T> : IMartenUser
    {
        private readonly Func<T> _idFunc;
        private T _id;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        /// <summary>
        /// Initializes a new instance of the <see cref="MartenUser{T}" /> class.
        /// </summary>
        /// <param name="idFunc">The identifier function.</param>
        public MartenUser(Func<T> idFunc) => _idFunc = idFunc;
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public object? Id => _id?.Equals(default(T)!) == null || _id?.Equals(default(T)!) == true
            ? _id = _idFunc()!
            : _id!;
    }
}