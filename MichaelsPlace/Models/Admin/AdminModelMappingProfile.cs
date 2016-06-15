using System;
using System.Linq;
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
                .IgnoreAllNonExisting()
                .ReverseMap()
                .ForMember(p => p.Id, o => o.Condition((ResolutionContext rc) => rc.DestinationValue == null))
                .IgnoreAllNonExisting();

            CreateMap<ApplicationUser, UserModel>()
                .ForMember(um => um.IsLockedOut, o => o.MapFrom(au => au.LockoutEndDateUtc != null && au.LockoutEndDateUtc > DateTime.UtcNow))
                .ForMember(um => um.IsDisabled, o => o.MapFrom(au => au.LockoutEndDateUtc == Constants.Magic.DisabledLockoutEndDate))
                .ForMember(um => um.IsStaff, o => o.MapFrom(au => au.Claims.Any(c => c.ClaimType == Constants.Claims.Staff)))
                .ForMember(pm => pm.IsEmailConfirmed, o => o.MapFrom(p => p.EmailConfirmed))
                .ForMember(pm => pm.IsPhoneNumberConfirmed, o => o.MapFrom(p => p.PhoneNumberConfirmed))
                .ForMember(m => m.Roles, o => o.MapFrom(au => au.Roles.Select(r => r.RoleId)))
                .IgnoreAllNonExisting()
                .ReverseMap()
                .ForMember(p => p.Id, o => o.Condition((ResolutionContext rc) => rc.DestinationValue == null))
                .ForMember(p => p.Roles, o => o.Ignore())
                .IgnoreAllNonExisting();

        }
    }
}