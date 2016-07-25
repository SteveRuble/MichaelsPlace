using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Ninject;

namespace MichaelsPlace.Controllers.Admin
{

    public class TagController : AdminControllerBase
    {
        private Injected<ApplicationDbContext> _dbContext;

        [Inject]
        public ApplicationDbContext DbContext
        {
            get { return _dbContext.Value; }
            set { _dbContext.Value = value; }
        }

        private Injected<IMapper> _mapper;

        [Inject]
        public IMapper Mapper
        {
            get { return _mapper.Value; }
            set { _mapper.Value = value; }
        }



        // GET: Tag
        public ActionResult Index()
        {
            var contextTagList = GetContextTagList();

            return View(contextTagList);
        }

        private List<AdminTagModel> GetContextTagList()
        {
            var tags = DbContext.Tags.ToList();

            var models = tags.OfType<ContextTag>().Select(Mapper.Map<AdminTagModel>)
                             .Concat(tags.OfType<RelationshipTag>().Select(Mapper.Map<AdminTagModel>))
                             .Concat(tags.OfType<LossTag>().Select(Mapper.Map<AdminTagModel>))
                             .ToList();


            return models;
        }
    }
}