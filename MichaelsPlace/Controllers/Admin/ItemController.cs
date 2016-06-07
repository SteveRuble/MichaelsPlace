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
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Controllers.Admin
{
    public class ItemController<TEntity, TModel> : AdminControllerBase
        where TEntity : Item
        where TModel : AdminItemModel
    {
        public virtual string Prefix => "Item";

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewBag.Prefix = Prefix;
            base.OnResultExecuting(filterContext);
        }

        public virtual async Task<ActionResult> Index()
        {
            var data = await DbContext.Set<TEntity>()
                                      .OrderBy(o => o.Order)
                                      .ProjectTo<TModel>(Mapper.ConfigurationProvider)
                                      .ToListAsync();

            return View($"~/Views/Item/Index.cshtml", data);
        }

        public virtual async Task<ActionResult> Programs()
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
            return View(article);
        }

        private async Task<TModel> FindItem(int? id)
        {
            var article = await DbContext.Set<TEntity>().Where(i => i.Id == id).ProjectTo<TModel>(Mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return article;
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
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
                return RedirectToAction("Index");
            }

            return View(model);
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
            return View(model);
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
                var article = await FindItem(model.Id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                Mapper.Map(model, article);
                DbContext.Entry(article).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Articles/Delete/5
        public virtual async Task<ActionResult> Delete(int? id)
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

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> DeleteConfirmed(int id)
        {
            Article article = (Article) await DbContext.Items.FindAsync(id);
            DbContext.Items.Remove(article);
            await DbContext.SaveChangesAsync();
            return RedirectToAction("Index");
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
