using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommunityHealth.Models;

namespace CommunityHealth.Controllers
{
    public class TypeController : Controller
    {
        private CommunityProgramsDBEntities db = new CommunityProgramsDBEntities();

        // GET: /Type/
        public async Task<ActionResult> Index()
        {
            return View(await db.ActivityTypes.ToListAsync());
        }

        // GET: /Type/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityType activitytype = await db.ActivityTypes.FindAsync(id);
            if (activitytype == null)
            {
                return HttpNotFound();
            }
            return View(activitytype);
        }

        // GET: /Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="TypeID,TypeName,isActive,RecordVersion")] ActivityType activitytype)
        {
            if (ModelState.IsValid)
            {
                db.ActivityTypes.Add(activitytype);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(activitytype);
        }

        // GET: /Type/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityType activitytype = await db.ActivityTypes.FindAsync(id);
            if (activitytype == null)
            {
                return HttpNotFound();
            }
            return View(activitytype);
        }

        // POST: /Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="TypeID,TypeName,isActive,RecordVersion")] ActivityType activitytype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activitytype).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(activitytype);
        }

        // GET: /Type/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityType activitytype = await db.ActivityTypes.FindAsync(id);
            if (activitytype == null)
            {
                return HttpNotFound();
            }
            return View(activitytype);
        }

        // POST: /Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ActivityType activitytype = await db.ActivityTypes.FindAsync(id);
            db.ActivityTypes.Remove(activitytype);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
