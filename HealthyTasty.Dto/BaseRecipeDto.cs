using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthyTasty.Infrastructure.Enums;

namespace HealthyTasty.Dto
{
    public class BaseRecipeDto : BaseDto
    {
        public CategoryDto Category { get; set; }
        public string Difficulty { get; set; }
        public int PreparationTime { get; set; }
    }
}
