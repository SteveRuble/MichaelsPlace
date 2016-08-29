using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using MichaelsPlace.Extensions;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Controllers.Admin
{
    public class OrganizationController : AdminControllerBase
    {
        // GET: Organization
        public async Task<ActionResult> Index()
        {
            return View(await DbContext.Organizations.ToListAsync());
        }

        public async Task<JsonResult> JsonIndex([ModelBinder(typeof(DataTables.AspNet.Mvc5.ModelBinder))] IDataTablesRequest requestModel)
        {
            var models = DbContext.Organizations;

            var response = requestModel.ApplyTo(models);

            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }

        // GET: Organization/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = await DbContext.Organizations.FindAsync(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // GET: Organization/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organization/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,Name,PhoneNumber,FaxNumber,Notes")] Organization organization)
        public async Task<ActionResult> Create(Organization organization)
        {
            //await SaveAddress(organization);
            ModelState["Address.ID"].Errors.Clear();
            if (ModelState.IsValid)
            {
                DbContext.Organizations.Add(organization);
                await DbContext.SaveChangesAsync();
                return Accepted();
            }

            return View(organization);
        }

        // GET: Organization/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = await DbContext.Organizations.FindAsync(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            if (organization.Address == null)
            {
                organization.Address = new Address();
            }

            return View(organization);
        }

        // POST: Organization/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Organization organization)
       {
            //organization.Address =  SaveAddress(organization);

            if (ModelState.IsValid)
            {
                //TODO: Users Assign  people to an organization roles within the organization
                DbContext.Entry(organization).State = EntityState.Modified;
                DbContext.Entry(organization.Address).State = organization.Address.Id == 0 ? EntityState.Added : EntityState.Modified;
                try
                {
                    DbContext.SaveChanges();
                    //await DbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
                
                return Accepted();
            }
            return View(organization);
        }

        //[ValidateAntiForgeryToken]
        // POST: Organization/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Organization organization = await DbContext.Organizations.FindAsync(id);
            DbContext.Organizations.Remove(organization);
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
}
