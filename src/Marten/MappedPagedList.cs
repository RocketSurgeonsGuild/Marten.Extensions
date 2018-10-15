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

        public int Count => _pagedList.Count;

        public int PageNumber => _pagedList.PageNumber;

        public int PageSize => _pagedList.PageSize;

        public int PageCount => _pagedList.PageCount;

        public int TotalItemCount => _pagedList.TotalItemCount;

        public bool HasPreviousPage => _pagedList.HasPreviousPage;

        public bool HasNextPage => _pagedList.HasNextPage;

        public bool IsFirstPage => _pagedList.IsFirstPage;

        public bool IsLastPage => _pagedList.IsLastPage;

        public int FirstItemOnPage => _pagedList.FirstItemOnPage;

        public int LastItemOnPage => _pagedList.LastItemOnPage;

        private T[] _values;
        public IEnumerable<T> Values => _values ?? (_values = this.ToArray());
    }
}
