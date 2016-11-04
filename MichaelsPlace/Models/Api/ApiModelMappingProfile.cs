using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MichaelsPlace.Models.Api.CaseDashboard;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api
{
    public class ApiModelMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Item, BrowsingItemModel>()
                .ForSourceMember(m => m.CreatedBy, o => o.Ignore())
                .ForSourceMember(m => m.CreatedUtc, o => o.Ignore());

            CreateMap<Article, BrowsingItemModel>()
                .ForSourceMember(m => m.CreatedBy, o => o.Ignore())
                .ForSourceMember(m => m.CreatedUtc, o => o.Ignore())
                .IncludeBase<Item, BrowsingItemModel>();

            CreateMap<ToDo, BrowsingItemModel>()
                .ForSourceMember(m => m.CreatedBy, o => o.Ignore())
                .ForSourceMember(m => m.CreatedUtc, o => o.Ignore())
                .IncludeBase<Item, BrowsingItemModel>();

            CreateMap<Case, CaseListModel>()
                .ForSourceMember(m => m.CreatedBy, o => o.Ignore())
                .ForSourceMember(m => m.CreatedUtc, o => o.Ignore());

            CreateMap<Case, CaseViewModel>()
                .ForSourceMember(m => m.CreatedBy, o => o.Ignore())
                .ForSourceMember(m => m.CreatedUtc, o => o.Ignore())
                .ForMember(m => m.Todos, o => o.MapFrom(s => s.CaseItems.Where(ci => ci.Item is ToDo)))
                .ForMember(m => m.Articles, o => o.MapFrom(s => s.CaseItems.Where(ci => ci.Item is Article)));

            CreateMap<PersonCase, PersonViewModel>()
                .ForMember(m => m.DisplayName, opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
                .ForMember(m => m.PersonItems, opt => opt.MapFrom(src => src.Person.PersonCaseItems.Where(pci => pci.Case.Id == src.Case.Id)))
                .ForSourceMember(m => m.Person, o => o.Ignore())
                .ForSourceMember(m => m.Relationship, o => o.Ignore())
                .ForSourceMember(m => m.Case, o => o.Ignore());

            CreateMap<PersonCaseItem, PersonItemViewModel>()
                .ForSourceMember(m => m.Person, o => o.Ignore());


            CreateMap<CaseItem, ItemViewModel>();

        }
    }
}
