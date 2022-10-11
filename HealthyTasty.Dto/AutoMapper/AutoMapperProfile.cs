using AutoMapper;
using HealthyTasty.Domain.Tables;

namespace HealthyTasty.Dto.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Recipe, RecipeDto>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
