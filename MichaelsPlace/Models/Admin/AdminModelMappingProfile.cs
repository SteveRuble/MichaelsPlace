using System;
using AutoMapper;
using MichaelsPlace.Extensions;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api
{
    public class AdminModelMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Item, AdminItemModel>()
                .ForMember(m => m.CreatedBy, o => o.MapFrom(i => i.CreatedBy));

            CreateMap<Article, AdminArticleModel>()
                .IncludeBase<Item, AdminItemModel>();

            CreateMap<ToDo, AdminToDoModel>()
                .IncludeBase<Item, AdminItemModel>();

            CreateMap<AdminItemModel, Item>()
                .ForMember(a => a.CreatedBy, o => o.Ignore())
                .ForMember(a => a.CreatedUtc, o => o.Ignore())
                .ForMember(a => a.Situations, o => o.Ignore())
                .ForMember(a => a.IsDeleted, o => o.Ignore())
                ;

            CreateMap<AdminArticleModel, Article>()
                .ForMember(a => a.CreatedBy, o => o.Ignore())
                .ForMember(a => a.CreatedUtc, o => o.Ignore())
                .ForMember(a => a.Situations, o => o.Ignore())
                .ForMember(a => a.IsDeleted, o => o.Ignore())
                ;

            CreateMap<AdminToDoModel, ToDo>()
                .ForMember(a => a.CreatedBy, o => o.Ignore())
                .ForMember(a => a.CreatedUtc, o => o.Ignore())
                .ForMember(a => a.Situations, o => o.Ignore())
                .ForMember(a => a.IsDeleted, o => o.Ignore())
                ;

            CreateMap<Person, PersonModel>()
                .ForMember(um => um.IsLockedOut, o => o.MapFrom(au => au.ApplicationUser.LockoutEndDateUtc != null && au.ApplicationUser.LockoutEndDateUtc > DateTime.UtcNow))
                .ForMember(um => um.IsDisabled, o => o.MapFrom(au => au.ApplicationUser.LockoutEndDateUtc == Constants.Magic.DisabledLockoutEndDate))
                .IgnoreAllNonExisting()
                .ReverseMap()
                .IgnoreAllNonExisting();
        }
    }
}