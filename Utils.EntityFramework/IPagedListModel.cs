using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.EntityFramework
{
    public interface IPagedListModel<out T>
    {
        IReadOnlyList<T> Data { get; }
        int Count { get; }
        int PageNumber { get; }
        int PageSize { get; }
        int TotalCount { get; set; }
        int PageCount { get; }
    }
}
