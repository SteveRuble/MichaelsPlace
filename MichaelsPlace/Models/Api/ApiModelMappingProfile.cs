using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api
{
    public class ApiModelMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Article, BrowsingItemModel>()
                .ForSourceMember(m => m.CreatedBy, o => o.Ignore())
                .ForSourceMember(m => m.CreatedUtc, o => o.Ignore());
        }
    }
}
