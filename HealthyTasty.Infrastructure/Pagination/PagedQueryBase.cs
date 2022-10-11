using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyTasty.Infrastructure.Pagination
{
    public class PagedQueryBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
