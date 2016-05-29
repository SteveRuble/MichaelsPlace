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
    public class ArticlesController : AdminControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var data = await DbContext.Items.OfType<Article>().ProjectTo<AdminArticleModel>(Mapper.ConfigurationProvider).ToListAsync();

            return View("~/Views/Shared/Index.cshtml", data);
        }

        // GET: Articles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = await FindArticle(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Shared/Details.cshtml", article);
        }

        private async Task<AdminArticleModel> FindArticle(int? id)
        {
            var article = await DbContext.Items.OfType<Article>().Where(i => i.Id == id).ProjectTo<AdminArticleModel>(Mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return article;
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View("~/Views/Shared/Create.cshtml", new AdminArticleModel());
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Content,Order")] AdminArticleModel model)
        {
            if (ModelState.IsValid)
            {
                var article = Mapper.Map<Article>(model);
                DbContext.Items.Add(article);
                await DbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("~/Views/Shared/Create.cshtml", model);
        }

        // GET: Articles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = await FindArticle(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Shared/Create.cshtml", model);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Content,Order")] AdminArticleModel model)
        {
            if (ModelState.IsValid)
            {
                var article = (Article)await DbContext.Items.FindAsync(model.Id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                Mapper.Map(model, article);
                DbContext.Entry(article).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("~/Views/Shared/Create.cshtml", model);
        }

        // GET: Articles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = await FindArticle(id);
            if (article == null)
            {
                return HttpNotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
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
}
