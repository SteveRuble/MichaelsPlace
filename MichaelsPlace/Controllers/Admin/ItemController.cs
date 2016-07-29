using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using MichaelsPlace.Extensions;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;
using MichaelsPlace.Utilities;
using Newtonsoft.Json;
using Ninject;
using Ninject.Infrastructure.Language;

namespace MichaelsPlace.Controllers.Admin
{
    public class TaggedItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<int> ContextTagIds { get; set; }
        public IEnumerable<int> LossTagIds { get; set; }
        public IEnumerable<int> RelationshipTagIds { get; set; }
    }

    public class ItemTaggingModel
    {
        public List<TaggedItemModel> Items { get; set; } = new List<TaggedItemModel>();
        public List<AdminTagModel> Tags { get; set; } = new List<AdminTagModel>();
        public bool? SaveSuccessful { get; set; }
    }

    public class ItemController<TEntity, TModel> : AdminControllerBase
        where TEntity : Item
        where TModel : AdminItemModel
    {
        public virtual string Prefix => "Item";

        private string ControllerName { get { return RouteData?.GetRequiredString("controller"); } }

        private Injected<AdminTagModelQuery> _adminTagModelQuery;

        [Inject]
        public AdminTagModelQuery AdminTagModelQuery
        {
            get { return _adminTagModelQuery.Value; }
            set { _adminTagModelQuery.Value = value; }
        }

        
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewBag.Prefix = Prefix;
            base.OnResultExecuting(filterContext);
        }
        
        public virtual async Task<ActionResult> Index()
        {
            var data = Enumerable.Empty<TModel>();

            return View($"~/Views/Item/Index.cshtml", data);
        }
        
        public async Task<JsonResult> JsonIndex([ModelBinder(typeof(DataTables.AspNet.Mvc5.ModelBinder))] IDataTablesRequest requestModel)
        {
            var models = DbContext.Set<TEntity>()
                                  .OrderBy(o => o.Order)
                                  .ProjectTo<TModel>(Mapper.ConfigurationProvider);

            var response = requestModel.ApplyTo(models);

            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }


        public virtual async Task<ActionResult> Tagging()
        {
            var data = await DbContext.Set<TEntity>()
                                      .Select(e => new TaggedItemModel()
                                                   {
                                                       Id = e.Id,
                                                       Title = e.Title,
                                                       ContextTagIds = e.AppliesToContexts.Select(t => t.Id),
                                                       LossTagIds = e.AppliesToLosses.Select(t => t.Id),
                                                       RelationshipTagIds = e.AppliesToRelationships.Select(t => t.Id),
                                                   })
                                      .ToListAsync();
            var model = new ItemTaggingModel()
                        {
                            Items = data,
                            Tags = AdminTagModelQuery.GetAdminTagModels()
                        };

            return View($"~/Views/Item/Tagging.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Tagging(string json)
        {
            var model = JsonConvert.DeserializeObject<ItemTaggingModel>(json);

            try
            {
                var tags = await DbContext.Set<Tag>().ToDictionaryAsync(t => t.Id);
                var items = await DbContext.Set<TEntity>().Include(i => i.AppliesToContexts)
                                           .Include(i => i.AppliesToLosses)
                                           .Include(i => i.AppliesToRelationships)
                                           .ToListAsync();

                var updates = from item in items
                              join update in model.Items on item.Id equals update.Id
                              select new {item, update};

                foreach (var entry in updates)
                {
                    entry.item.AppliesToContexts.MirrorFrom(entry.update.ContextTagIds.Select(id => tags[id]).OfType<ContextTag>().ToList());
                    entry.item.AppliesToLosses.MirrorFrom(entry.update.LossTagIds.Select(id => tags[id]).OfType<LossTag>().ToList());
                    entry.item.AppliesToRelationships.MirrorFrom(entry.update.RelationshipTagIds.Select(id => tags[id]).OfType<RelationshipTag>().ToList());
                }

                DbContext.SaveChanges();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }

            model.Tags = AdminTagModelQuery.GetAdminTagModels();
            model.SaveSuccessful = ModelState.IsValid;

            return View($"~/Views/Item/Tagging.cshtml", model);
        }
        
        public virtual async Task<ActionResult> Ordering()
        {
            var data = await DbContext.Set<TEntity>()
                                      .Select(e => new OrderingModel()
                                                   {
                                                       Id = e.Id,
                                                       Order = e.Order,
                                                       Title = e.Title
                                                   })
                                      .OrderBy(o => o.Order)
                                      .ToListAsync();
            

            return View($"~/Views/Item/Ordering.cshtml", data);
        }

        [HttpPost]
        public virtual ActionResult Ordering(List<OrderingModel> items)
        {
            var data = DbContext.Set<TEntity>().ToDictionary(t => t.Id);

            foreach (var orderingModel in items)
            {
                var entity = data[orderingModel.Id];
                entity.Order = orderingModel.Order;
            }

            DbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Articles/Details/5
        public virtual async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = await FindItem(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View($"~/Views/{ControllerName}/Details.cshtml",article);
        }

        private async Task<TModel> FindItem(int? id)
        {
            var article = await DbContext.Set<TEntity>().Where(i => i.Id == id).ProjectTo<TModel>(Mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return article;
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View($"~/Views/{ControllerName}/Create.cshtml");
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create([Bind(Include = "Id,Title,Content,Order")] TModel model)
        {
            if (ModelState.IsValid)
            {
                var article = Mapper.Map<TEntity>(model);
                DbContext.Set<TEntity>().Add(article);
                await DbContext.SaveChangesAsync();
                return Accepted();
            }

            return View($"~/Views/{ControllerName}/Create.cshtml", model);
        }

        // GET: Articles/Edit/5
        public virtual async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = await FindItem(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View($"~/Views/{ControllerName}/Edit.cshtml", model);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit([Bind(Include = "Id,Title,Content,Order")] TModel model)
        {
            if (ModelState.IsValid)
            {
                var article = await DbContext.Set<TEntity>().FirstOrDefaultAsync(i => i.Id == model.Id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                Mapper.Map(model, article);
                DbContext.Entry(article).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                return Accepted();
            }
            return View($"~/Views/{ControllerName}/Edit.cshtml", model);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(int id)
        {
            Article article = (Article) await DbContext.Items.FindAsync(id);
            DbContext.Items.Remove(article);
            await DbContext.SaveChangesAsync();
            return Accepted();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class ArticleController : ItemController<Article, AdminArticleModel>
    {
        public override string Prefix => "Articles";
    }

    public class ToDoController : ItemController<ToDo, AdminToDoModel>
    {
        public override string Prefix => "To-Do";
        
    }
}
