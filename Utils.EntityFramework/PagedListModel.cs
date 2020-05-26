using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.EntityFramework
{
    /// <summary>
    /// 分页Model
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    [Serializable]
    public class PagedListModel<T> : IPagedListModel<T>
    {
        public static readonly IPagedListModel<T> Empty = new PagedListModel<T>();

        private IReadOnlyList<T> _data = ArrayHelper.Empty<T>();

        public IReadOnlyList<T> Data
        {
            get => _data;
            set
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (value != null)
                {
                    _data = value;
                }
            }
        }

        private int _pageNumber = 1;

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (value > 0)
                {
                    _pageNumber = value;
                }
            }
        }

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > 0)
                {
                    _pageSize = value;
                }
            }
        }

        private int _totalCount;

        public int TotalCount
        {
            get => _totalCount;
            set
            {
                if (value > 0)
                {
                    _totalCount = value;
                }
            }
        }

        public int PageCount => (_totalCount + _pageSize - 1) / _pageSize;

        public T this[int index] => Data[index];

        public int Count => Data.Count;
    }
}
