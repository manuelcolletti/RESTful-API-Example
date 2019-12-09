using AutoMapper;
using LogItUpApi.Entities;
using LogItUpApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogItUpApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryType, CategoryTypeDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
        }
    }
}
