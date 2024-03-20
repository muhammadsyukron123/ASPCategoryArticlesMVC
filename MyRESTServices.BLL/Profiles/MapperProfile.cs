using AutoMapper;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRESTServices.BLL.Profiles
{

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap(); 
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<CategoryUpdateDTO, Category>();
            CreateMap<Article, ArticleDTO>().ReverseMap();
            CreateMap<ArticleCreateDTO, Article>();
            CreateMap<ArticleUpdateDTO, Article>();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<RoleCreateDTO, Role>();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserCreateDTO, User>();
            CreateMap<LoginDTO, User>();
        }

    }
}
