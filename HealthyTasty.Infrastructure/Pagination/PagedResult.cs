using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace HealthyTasty.Infrastructure.Pagination
{
    public class PagedResult<T> : PagedResultInfo
    {
        public IEnumerable<T> Items { get; set; }

        private PagedResult() : base(0, 0 ,0 ,0)
        {
            Items = Enumerable.Empty<T>();
        }

        public PagedResult(IEnumerable<T> items, int pageIndex, int pageSize, int totalCount, int totalPages)
            : base(pageIndex, pageSize, totalCount, totalPages)
        {
            Items = items;
        }

        public static PagedResult<T> Convert<T2>(PagedResult<T2> from, IEnumerable<T> results)
            => new(results, from.PageIndex, from.PageSize, from.TotalCount, from.TotalPages);

        public static PagedResult<T> Empty => new();
    }
}
