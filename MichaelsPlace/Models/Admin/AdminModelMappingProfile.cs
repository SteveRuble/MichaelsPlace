using System;
using System.Data.Entity;
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
            ConfigureFromPersistenceToModel();

            ConfigureFromModelToPersistence();
        }

        private void ConfigureFromModelToPersistence()
        {
            CreateMap<AdminItemModel, Item>()
                .ForMember(a => a.CreatedBy, o => o.Ignore())
                .ForMember(a => a.CreatedUtc, o => o.Ignore())
                .ForMember(a => a.IsDeleted, o => o.Ignore())
                .ForMember(a => a.AppliesToContexts, o => o.Ignore())
                .ForMember(a => a.AppliesToRelationships, o => o.Ignore())
                .ForMember(a => a.AppliesToLosses, o => o.Ignore())
                ;

            CreateMap<AdminArticleModel, Article>()
                .IncludeBase<AdminItemModel, Item>();

            CreateMap<AdminToDoModel, ToDo>()
                .IncludeBase<AdminItemModel, Item>();

            #region Tags
            CreateMap<AdminTagModel, LossTag>()
                .ForMember(x => x.Context, o => o.Ignore())
                .ForMember(t => t.ContextId, o => o.MapFrom(s => s.ParentId));
            CreateMap<AdminTagModel, RelationshipTag>()
                .ForMember(x => x.Context, o => o.Ignore())
                .ForMember(t => t.ContextId, o => o.MapFrom(s => s.ParentId));
            CreateMap<AdminTagModel, ContextTag>()
                .ForMember(t => t.Losses, o => o.Ignore())
                .ForMember(t => t.Relationships, o => o.Ignore());
            #endregion Tags
        }

        private void ConfigureFromPersistenceToModel()
        {
            CreateMap<Item, AdminItemModel>()
                .ForMember(m => m.ContextTagIds, o => o.MapFrom(m => m.AppliesToContexts.Select(t => t.Id)))
                .ForMember(m => m.LossTagIds, o => o.MapFrom(m => m.AppliesToLosses.Select(t => t.Id)))
                .ForMember(m => m.RelationshipTagIds, o => o.MapFrom(m => m.AppliesToRelationships.Select(t => t.Id)))
                .ForMember(m => m.CreatedBy, o => o.MapFrom(i => i.CreatedBy));

            CreateMap<Article, AdminArticleModel>()
                .IncludeBase<Item, AdminItemModel>();

            CreateMap<ToDo, AdminToDoModel>()
                .IncludeBase<Item, AdminItemModel>();

            CreateMap<Person, PersonModel>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .ForMember(p => p.Id, o => o.Condition(rc => rc.DestinationValue == null))
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
                .ForMember(p => p.Id, o => o.Condition(rc => rc.DestinationValue == null))
                .ForMember(p => p.Roles, o => o.Ignore())
                .IgnoreAllNonExisting();

            #region Tags

            CreateMap<LossTag, AdminTagModel>()
                .ForMember(m => m.State, o => o.UseValue(EntityState.Unchanged))
                .ForMember(t => t.Type, o => o.UseValue(AdminTagType.Loss))
                .ForMember(m => m.ParentId, o => o.MapFrom(s => s.ContextId));
            CreateMap<RelationshipTag, AdminTagModel>()
                .ForMember(m => m.State, o => o.UseValue(EntityState.Unchanged))
                .ForMember(t => t.Type, o => o.UseValue(AdminTagType.Relationship))
                .ForMember(m => m.ParentId, o => o.MapFrom(s => s.ContextId));
            CreateMap<ContextTag, AdminTagModel>()
                .ForMember(m => m.State, o => o.UseValue(EntityState.Unchanged))
                .ForMember(t => t.Type, o => o.UseValue(AdminTagType.Context))
                .ForMember(m => m.ParentId, o => o.Ignore());

            #endregion Tags
        }
    }

    public enum AdminTagType
    {
        Unknown,
        Context,
        Loss,
        Relationship
    }

    public class AdminTagModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public EntityState State { get; set; }
        public AdminTagType Type { get; set; }
    }
}