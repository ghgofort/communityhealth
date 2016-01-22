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
    public class PopulationController : Controller
    {
        private CommunityProgramsDBEntities db = new CommunityProgramsDBEntities();

        // GET: /Population/
        public async Task<ActionResult> Index()
        {
            return View(await db.TargetPopulations.ToListAsync());
        }

        // GET: /Population/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetPopulation targetpopulation = await db.TargetPopulations.FindAsync(id);
            if (targetpopulation == null)
            {
                return HttpNotFound();
            }
            return View(targetpopulation);
        }

        // GET: /Population/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Population/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="PopulationName,isActive")] TargetPopulation targetpopulation)
        {
            if (ModelState.IsValid)
            {
                db.TargetPopulations.Add(targetpopulation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(targetpopulation);
        }

        // GET: /Population/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetPopulation targetpopulation = await db.TargetPopulations.FindAsync(id);
            if (targetpopulation == null)
            {
                return HttpNotFound();
            }
            return View(targetpopulation);
        }

        // POST: /Population/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="PopulationID,PopulationName,isActive,RecordVersion")] TargetPopulation targetpopulation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(targetpopulation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(targetpopulation);
        }

        // GET: /Population/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetPopulation targetpopulation = await db.TargetPopulations.FindAsync(id);
            if (targetpopulation == null)
            {
                return HttpNotFound();
            }
            return View(targetpopulation);
        }

        // POST: /Population/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TargetPopulation targetpopulation = await db.TargetPopulations.FindAsync(id);
            db.TargetPopulations.Remove(targetpopulation);
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
