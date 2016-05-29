using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api
{
    public class AdminModelMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Item, AdminItemModel>()
                .ForMember(m => m.CreatedBy, o => o.MapFrom(i => i.CreatedBy.UserName));

            CreateMap<Article, AdminArticleModel>()
                .IncludeBase<Item, AdminItemModel>();

            CreateMap<ToDo, AdminToDoModel>()
                .IncludeBase<Item, AdminItemModel>();

            CreateMap<AdminItemModel, Item>()
                .ForMember(a => a.CreatedBy, o => o.Ignore())
                .ForMember(a => a.CreatedUtc, o => o.Ignore())
                .ForMember(a => a.Situations, o => o.Ignore());

            CreateMap<AdminArticleModel, Article>()
                .IncludeBase<AdminItemModel, Item>();

            CreateMap<AdminToDoModel, ToDo>()
                .IncludeBase<AdminItemModel, Item>();


        }
    }
}
