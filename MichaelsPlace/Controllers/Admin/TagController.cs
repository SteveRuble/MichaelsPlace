using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MichaelsPlace.CommandHandlers;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Newtonsoft.Json;
using Ninject;

namespace MichaelsPlace.Controllers.Admin
{

    public class AdminTagEditModel
    {
        public List<AdminTagModel> Tags { get; set; } = new List<AdminTagModel>();

        public bool? SaveSuccessful { get; set; }
        public string Message { get; set; }
    }

    public class TagController : AdminControllerBase
    {
        // GET: Tag
        public ActionResult Index()
        {
            var tags = GetAdminTagModels();


            return View(new AdminTagEditModel()
                        {
                            Tags = tags
                        });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string json)
        {
            var model = JsonConvert.DeserializeObject<AdminTagEditModel>(json);
            try
            {
                var command = new UpdateTagsCommand(model.Tags, ModelState);
                var result = await Mediator.SendAsync(command);

                model.SaveSuccessful = true;
                model.Message = "Tags Updated!";
                model.Tags = GetAdminTagModels();
            }
            catch (Exception ex)
            {
                model.SaveSuccessful = false;
                model.Message = ex.ToString();
            }

            return View(model);
        }

        private List<AdminTagModel> GetAdminTagModels()
        {
            var tags = DbContext.Tags.ToList();

            var models =
                tags.OfType<ContextTag>()
                    .Select(Mapper.Map<AdminTagModel>)
                    .Concat(tags.OfType<LossTag>().Select(Mapper.Map<AdminTagModel>))
                    .Concat(tags.OfType<RelationshipTag>().Select(Mapper.Map<AdminTagModel>))
                    .ToList();

            return models;
        }
    }
}