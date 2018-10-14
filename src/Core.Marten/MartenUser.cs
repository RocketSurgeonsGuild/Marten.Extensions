using System;

namespace Rocket.Surgery.Core.Marten
{
    public class MartenUser<T> : IMartenUser
    {
        private readonly Func<T> _idFunc;
        private T _id;

        public MartenUser(Func<T> idFunc)
        {
            _idFunc = idFunc;
        }

        public object Id => _id?.Equals(default(T)) == null || _id?.Equals(default(T)) == true ? _id = _idFunc() : _id;
    }
}
