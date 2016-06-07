using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                .ForMember(m => m.CreatedBy, o => o.MapFrom(i => i.CreatedBy));

            CreateMap<Article, AdminArticleModel>()
                .IncludeBase<Item, AdminItemModel>();

            CreateMap<ToDo, AdminToDoModel>()
                .IncludeBase<Item, AdminItemModel>();

            CreateMap<AdminItemModel, Item>()
                .ForMember(a => a.CreatedBy, o => o.Ignore())
                .ForMember(a => a.CreatedUtc, o => o.Ignore())
                .ForMember(a => a.Situations, o => o.Ignore());

            CreateMap<AdminArticleModel, Article>()
                .ForMember(a => a.CreatedBy, o => o.Ignore())
                .ForMember(a => a.CreatedUtc, o => o.Ignore())
                .ForMember(a => a.Situations, o => o.Ignore());

            CreateMap<AdminToDoModel, ToDo>()
                .ForMember(a => a.CreatedBy, o => o.Ignore())
                .ForMember(a => a.CreatedUtc, o => o.Ignore())
                .ForMember(a => a.Situations, o => o.Ignore());

            CreateMap<Person, PersonModel>()
                .ForMember(um => um.IsLockedOut, o => o.MapFrom(au => au.ApplicationUser.LockoutEndDateUtc != null && au.ApplicationUser.LockoutEndDateUtc < DateTime.UtcNow))
                .IgnoreAllNonExisting()
                .ReverseMap()
                .IgnoreAllNonExisting();
        }
    }

    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
            (this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }
    }
}
