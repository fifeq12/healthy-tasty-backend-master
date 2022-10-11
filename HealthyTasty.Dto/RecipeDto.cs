using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyTasty.Dto
{
    public class RecipeDto : BaseRecipeDto
    {
        public string? Description { get; set; }
        public int Servings { get; set; }
    }
}
