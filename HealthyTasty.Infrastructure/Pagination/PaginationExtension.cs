using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HealthyTasty.Infrastructure.Pagination
{
    public static class PaginationExtension
    {
        public static async Task<PagedResult<T>> Paginate<T>(this IQueryable<T> items, PagedQueryBase query)
        {
            var totalResults = await items.CountAsync();

            if (totalResults == 0)
                return PagedResult<T>.Empty;

            if (query.PageIndex <= 0)
                query.PageIndex = 1;

            if (query.PageSize <= 0)
                query.PageSize = 10;

            if(query.PageSize > 200)
                query.PageSize = 200;

            var totalPages = (int)Math.Ceiling((decimal)totalResults / query.PageSize);

            if(query.PageIndex > totalPages)
                query.PageIndex = totalPages;

            var result = await items.Skip((query.PageIndex - 1) * query.PageSize)
                .Take(query.PageSize).ToListAsync();

            return new PagedResult<T>(result, query.PageIndex, 
                query.PageSize, totalResults, totalPages);
        }
    }
}
