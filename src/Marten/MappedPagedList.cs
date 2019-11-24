using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Marten.Pagination;
using Newtonsoft.Json;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// MappedPagedList.
    /// Implements the <see cref="IPagedList{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IPagedList{T}" />
    [JsonObject]
    public class MappedPagedList<T> : IPagedList<T>
    {
        private readonly IPagedList<object> _pagedList;
        private readonly Func<object, T> _mapper;

        private T[]? _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedPagedList{T}" /> class.
        /// </summary>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="mapper">The mapper.</param>
        public MappedPagedList(IPagedList<object> pagedList, Func<object, T> mapper)
        {
            _pagedList = pagedList;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<T> Values => _values ?? ( _values = this.ToArray() );

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() => _pagedList.Select(_mapper).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ( (IEnumerable)_pagedList ).GetEnumerator();

        /// <summary>
        /// Gets the at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>T.</returns>
        public T this[int index] => _mapper(_pagedList[index]);

        /// <summary>
        /// Return the number of records in the paged query result
        /// </summary>
        /// <value>The count.</value>
        public long Count => _pagedList.Count;

        /// <summary>
        /// Gets current page number
        /// </summary>
        /// <value>The page number.</value>
        public long PageNumber => _pagedList.PageNumber;

        /// <summary>
        /// Gets page size
        /// </summary>
        /// <value>The size of the page.</value>
        public long PageSize => _pagedList.PageSize;

        /// <summary>
        /// Gets number of pages
        /// </summary>
        /// <value>The page count.</value>
        public long PageCount => _pagedList.PageCount;

        /// <summary>
        /// Gets the total number records
        /// </summary>
        /// <value>The total item count.</value>
        public long TotalItemCount => _pagedList.TotalItemCount;

        /// <summary>
        /// Gets a value indicating whether there is a previous page
        /// </summary>
        /// <value><c>true</c> if this instance has previous page; otherwise, <c>false</c>.</value>
        public bool HasPreviousPage => _pagedList.HasPreviousPage;

        /// <summary>
        /// Gets a value indicating whether there is next page
        /// </summary>
        /// <value><c>true</c> if this instance has next page; otherwise, <c>false</c>.</value>
        public bool HasNextPage => _pagedList.HasNextPage;

        /// <summary>
        /// Gets a value indicating whether the current page is first page
        /// </summary>
        /// <value><c>true</c> if this instance is first page; otherwise, <c>false</c>.</value>
        public bool IsFirstPage => _pagedList.IsFirstPage;

        /// <summary>
        /// Gets a value indicating whether the current page is last page
        /// </summary>
        /// <value><c>true</c> if this instance is last page; otherwise, <c>false</c>.</value>
        public bool IsLastPage => _pagedList.IsLastPage;

        /// <summary>
        /// Gets one-based index of first item in current page
        /// </summary>
        /// <value>The first item on page.</value>
        public long FirstItemOnPage => _pagedList.FirstItemOnPage;

        /// <summary>
        /// Gets one-based index of last item in current page
        /// </summary>
        /// <value>The last item on page.</value>
        public long LastItemOnPage => _pagedList.LastItemOnPage;
    }
}