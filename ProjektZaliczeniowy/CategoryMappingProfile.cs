using AutoMapper;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CreateCategoryDto, Category>();
        }
    }
}
