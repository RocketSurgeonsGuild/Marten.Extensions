using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Marten.Pagination;
using Newtonsoft.Json;

namespace Rocket.Surgery.Extensions.Marten
{
    [JsonObject]
    public class MappedPagedList<T> : IPagedList<T>
    {
        private readonly IPagedList<object> _pagedList;
        private readonly Func<object, T> _mapper;

        public MappedPagedList(IPagedList<object> pagedList, Func<object, T> mapper)
        {
            _pagedList = pagedList;
            _mapper = mapper;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _pagedList.Select(_mapper).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_pagedList).GetEnumerator();
        }

        public T this[int index] => _mapper(_pagedList[index]);

        public long Count => _pagedList.Count;

        public long PageNumber => _pagedList.PageNumber;

        public long PageSize => _pagedList.PageSize;

        public long PageCount => _pagedList.PageCount;

        public long TotalItemCount => _pagedList.TotalItemCount;

        public bool HasPreviousPage => _pagedList.HasPreviousPage;

        public bool HasNextPage => _pagedList.HasNextPage;

        public bool IsFirstPage => _pagedList.IsFirstPage;

        public bool IsLastPage => _pagedList.IsLastPage;

        public long FirstItemOnPage => _pagedList.FirstItemOnPage;

        public long LastItemOnPage => _pagedList.LastItemOnPage;

        private T[] _values;
        public IEnumerable<T> Values => _values ?? (_values = this.ToArray());
    }
}
