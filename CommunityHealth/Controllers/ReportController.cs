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
using CommunityHealth.Models.ViewModels;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPPlusEnumerable;

namespace CommunityHealth.Controllers
{
    public class ReportController : Controller
    {
        private CommunityProgramsDBEntities db = new CommunityProgramsDBEntities();

        // GET: /Report/
        public async Task<ActionResult> Index()
        {
            List<ReportBuilderViewModel> rbvmList = new List<ReportBuilderViewModel>();
            List<Report> rpts = await db.Reports.ToListAsync();
            foreach (Report rpt in rpts)
            {
                ReportBuilderViewModel rbvm = new ReportBuilderViewModel()
                {
                    report=rpt,
                    junctionReportDepartments = await db.JunctionReportDepartments.Where(m => m.ReportID == rpt.ReportID).ToListAsync(),
                    junctionReportLocations = await db.JunctionReportLocations.Where(m => m.ReportID == rpt.ReportID).ToListAsync(),
                    junctionReportPopulations = await db.JunctionReportPopulations.Where(m => m.ReportID == rpt.ReportID).ToListAsync(),
                    junctionReportPrograms = await db.JunctionReportPrograms.Where(m => m.ReportID == rpt.ReportID).ToListAsync(),
                    junctionReportRoles = await db.JunctionReportRoles.Where(m => m.ReportID == rpt.ReportID).ToListAsync(),
                    junctionReportTypes = await db.JunctionReportTypes.Where(m => m.ReportID == rpt.ReportID).ToListAsync()
                };
                rbvmList.Add(rbvm);
            }
            return View(rbvmList.OrderByDescending(m=>m.report.ReportID));
        }

        private List<ActivityReportModel> getModel(ReportBuilderViewModel rbvm)
        {
            IQueryable<CommunityAction> query = db.CommunityActions;

            // Sort by Date
            if(rbvm.report.SortByDate)
            {
                query = query.Where(r => r.StartDate<=rbvm.report.EndReportDate && r.StartDate>=rbvm.report.BeginReportDate);
            }
            // Sort by Location
            if(rbvm.report.SortByLocation)
            {
                List<int> ids = new List<int>();
                foreach(JunctionReportLocation jrl in rbvm.junctionReportLocations)
                {
                    // Get list of ID's
                    ids.Add(jrl.LocationID);
                }
                query = query.Where(r => ids.Contains(r.LocationID));
            }
            // Sort by Program
            if(rbvm.report.SortByProgram)
            {
                List<int> ids = new List<int>();
                foreach(JunctionReportProgram jrp in rbvm.junctionReportPrograms)
                {
                    ids.Add(jrp.ProgramID);
                }
                query = query.Where(r => ids.Contains((int)r.ProgramID));
            }
            // Sort by Population
            if (rbvm.report.SortByPopulation)
            {
                List<int> ids = new List<int>();
                foreach (JunctionReportPopulation jrp in rbvm.junctionReportPopulations)
                {
                    ids.Add(jrp.PopulationID);
                }

                List<JunctionPopulationAction> jpaList = db.JunctionPopulationActions.Where(r => ids.Contains((int)r.PopulationID)).ToList();
                List<int> intActionID = new List<int>();
                foreach(JunctionPopulationAction jpa in jpaList)
                {
                    intActionID.Add(jpa.ActionID);
                }
                query = query.Where(r => intActionID.Contains(r.ActionID));
            }
            // Sort by Program
            if (rbvm.report.SortByProgram)
            {
                List<int> ids = new List<int>();
                foreach (JunctionReportProgram jrp in rbvm.junctionReportPrograms)
                {
                    ids.Add(jrp.ProgramID);
                }
                query = query.Where(r => ids.Contains((int)r.ProgramID));
            }
            // Sort by Program
            if (rbvm.report.SortByProgram)
            {
                List<int> ids = new List<int>();
                foreach (JunctionReportProgram jrp in rbvm.junctionReportPrograms)
                {
                    ids.Add(jrp.ProgramID);
                }
                query = query.Where(r => ids.Contains((int)r.ProgramID));
            }
            // Sort by Program
            if (rbvm.report.SortByProgram)
            {
                List<int> ids = new List<int>();
                foreach (JunctionReportProgram jrp in rbvm.junctionReportPrograms)
                {
                    ids.Add(jrp.ProgramID);
                }
                query = query.Where(r => ids.Contains((int)r.ProgramID));
            }
            List<CommunityAction> queryList = query.ToList();
            List<ActivityReportModel> avm = new List<ActivityReportModel>();
            foreach(CommunityAction ca in queryList)
            {
                
                // Add Types
                foreach (JunctionTypeAction jta in ca.JunctionTypeActions)
                {
                    ActivityReportModel arm = new ActivityReportModel();
                    if (jta.ActivityType.TypeName.ToLower().Contains("other"))
                    {
                        if (jta.subType != null) arm.Type += jta.subType.ToString();
                    }
                    else
                    {
                        arm.Type += db.ActivityTypes.Find(jta.TypeID).TypeName.ToString();
                    }
                    if (jta != ca.JunctionTypeActions.Last()) arm.Type += ", ";
                    arm.Participants = jta.Participants.ToString();
                    arm.DescriptiveTitle = ca.BreifDescription;
                    arm.StartDate = ca.StartDate.ToShortDateString();
                    arm.EndDate = ca.EndDate.HasValue? ca.EndDate.Value.ToShortDateString(): "";
                    if (ca.Duration != null) arm.Duration = ca.Duration.ToString();
                    if(ca.Notes!=null)arm.Notes = ca.Notes;
                    if(ca.AddedBy!=null)arm.AddedBy = ca.AddedBy;
                    // Add Locations
                    if (ca.Location != null)
                    {
                        if (ca.Location.LocationName.ToLower().Contains("other"))
                        {
                            if(ca.SubLocation!=null)arm.Location = ca.SubLocation.ToString();
                        }
                        else
                        {
                            arm.Location = ca.Location.LocationName.ToString();
                        }
                    }
               
                    // Add Departments
                    foreach (JunctionDepartmentAction jda in ca.JunctionDepartmentActions)
                    {
                        if (jda.Department.DepartmentName.ToLower().Contains("other"))
                        {
                            if(jda.SubDepartment!=null)arm.Department += jda.SubDepartment.ToString();
                        }
                        else
                        {
                            arm.Department += jda.Department.DepartmentName;
                        }
                        if(jda != ca.JunctionDepartmentActions.Last())
                        {
                            arm.Department += ", ";
                        }
                    }
                    // Add Roles
                    foreach(JunctionRoleAction jra in ca.JunctionRoleActions)
                    {
                        if(jra.Role!=null)
                        {
                            arm.Role += jra.Role.RoleName;
                            if(jra != ca.JunctionRoleActions.Last())
                            {
                                arm.Role += ", ";
                            }
                        }
                    }
                    // Add Populations
                    foreach(JunctionPopulationAction jpa in ca.JunctionPopulationActions)
                    {
                        if(jpa.TargetPopulation !=null)
                        {
                            arm.Population += jpa.TargetPopulation.PopulationName;
                            if(jpa != ca.JunctionPopulationActions.Last())
                            {
                                arm.Population += ", ";
                            }
                        }
                    }
                    // Add Program
                    if (ca.Program != null)
                    {
                        if (ca.Program.ProgramName.ToLower().Contains("other"))
                        {
                            if (ca.SubProgram != null) arm.Program = ca.SubProgram.ToString();
                        }
                        else
                        {
                            arm.Program = ca.Program.ProgramName.ToString();
                        }
                    }
                    // Add Durations
                    avm.Add(arm);
                }
            }
            return avm;
        }
        // GET: /Report/Details/5
        public async Task<ActionResult> RunReport(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Create the report to be run
            ReportBuilderViewModel rbvm = new ReportBuilderViewModel()
            {
                report = await db.Reports.FindAsync(id),
                junctionReportDepartments = await db.JunctionReportDepartments.Where(m => m.ReportID==id).ToListAsync(),
                junctionReportLocations = await db.JunctionReportLocations.Where(m => m.ReportID == id).ToListAsync(),
                junctionReportPopulations = await db.JunctionReportPopulations.Where(m => m.ReportID == id).ToListAsync(),
                junctionReportTypes = await db.JunctionReportTypes.Where(m => m.ReportID == id).ToListAsync(),
                junctionReportRoles = await db.JunctionReportRoles.Where(m => m.ReportID == id).ToListAsync(),
                junctionReportPrograms = await db.JunctionReportPrograms.Where(m => m.ReportID == id).ToListAsync(),
            };
            if (rbvm.report == null)
            {
                return HttpNotFound();
            }

            // Call the getModel method to create the filtered list of activities in text form
            List<ActivityReportModel> arm = getModel(rbvm);
            // Turn the text list into the excel sheet and return the excel sheet
            if (arm.Count == 0)
            {
                arm.Add(new ActivityReportModel());
                arm.First().DescriptiveTitle = "There are no event community health records that match your sort or search criteria";
            }

            var bytes = Spreadsheet.Create(arm);
            return File(bytes, "application/vnc.ms-excel", "Report" + DateTime.Now.ToShortDateString()+".xlsx");
        }

        // GET: /Report/Create
        public ActionResult Create()
        {
            // Set the view bag variables for the drop downs
            SetViewBag();
            ReportBuilderViewModel rbvm = new ReportBuilderViewModel();
            rbvm.report = new Report();
            rbvm.beginningReportDate = System.DateTime.Now;
            rbvm.endReportDate = System.DateTime.Now;
            return View(rbvm);
        }

        // POST: /Report/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ReportBuilderViewModel rbvm)
        {
            //rbvm = CheckDummyValues(rbvm);
            if (ModelState.IsValid)
            {
                rbvm.report.BeginReportDate = rbvm.beginningReportDate;
                rbvm.report.EndReportDate = rbvm.endReportDate;
                db.Reports.Add(rbvm.report);
                // Check to see if any of the sort checkboxes are checked and write to junction tables 
                if(rbvm.report.SortByDepartment)
                {
                    foreach(JunctionReportDepartment jrd in rbvm.junctionReportDepartments)
                    {
                        db.JunctionReportDepartments.Add(jrd); 
                    } 
                }
                if (rbvm.report.SortByLocation)
                {
                    foreach (JunctionReportLocation jrl in rbvm.junctionReportLocations)
                    {
                        db.JunctionReportLocations.Add(jrl);
                    }
                }
                if (rbvm.report.SortByProgram)
                {
                    foreach (JunctionReportProgram jrp in rbvm.junctionReportPrograms)
                    {
                        db.JunctionReportPrograms.Add(jrp);
                    }
                }
                if (rbvm.report.SortByPopulation)
                {
                    foreach (JunctionReportPopulation jrp in rbvm.junctionReportPopulations)
                    {
                        db.JunctionReportPopulations.Add(jrp);
                    }
                }
                if (rbvm.report.SortByRole)
                {
                    foreach (JunctionReportRole jrr in rbvm.junctionReportRoles)
                    {
                        db.JunctionReportRoles.Add(jrr);
                    }
                }
                if (rbvm.report.SortByType)
                {
                    foreach (JunctionReportType jrt in rbvm.junctionReportTypes)
                    {
                        db.JunctionReportTypes.Add(jrt);
                    }
                }
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                SetViewBag();
            }

            return View(rbvm);
        }

        // GET: /Report/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportBuilderViewModel rbvm = new ReportBuilderViewModel
            {
                report = await db.Reports.FindAsync(id),
                junctionReportDepartments = await db.JunctionReportDepartments.Where(m => m.ReportID == id).ToListAsync(),
                junctionReportLocations = await db.JunctionReportLocations.Where(m => m.ReportID == id).ToListAsync(),
                junctionReportPopulations = await db.JunctionReportPopulations.Where(m => m.ReportID == id).ToListAsync(),
                junctionReportPrograms = await db.JunctionReportPrograms.Where(m => m.ReportID == id).ToListAsync(),
                junctionReportRoles = await db.JunctionReportRoles.Where(m => m.ReportID == id).ToListAsync(),
                junctionReportTypes = await db.JunctionReportTypes.Where(m => m.ReportID == id).ToListAsync()
            };
            rbvm.beginningReportDate = rbvm.report.BeginReportDate;
            rbvm.endReportDate = rbvm.report.EndReportDate;
            if (rbvm.report == null)
            {
                return HttpNotFound();
            }
            return View(rbvm);
        }

        // POST: /Report/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ReportBuilderViewModel rbvm)
        {
            if (ModelState.IsValid)
            {
                rbvm.report.BeginReportDate = rbvm.beginningReportDate;
                rbvm.report.EndReportDate = rbvm.endReportDate;
                db.Reports.Add(rbvm.report);
                /*
                int intHoldIndex = rbvm.report.ReportID;
                //db.Reports.Remove(db.Reports.Where(m => m.ReportID == intHoldIndex).FirstOrDefault());
                // Remove all of the old junctions out to recreate the item.
                List<JunctionReportDepartment> oldJRD = db.JunctionReportDepartments.Where(m => m.ReportID == intHoldIndex).ToList();
                db.JunctionReportDepartments.RemoveRange(oldJRD);
                List<JunctionReportLocation> oldJRL = db.JunctionReportLocations.Where(m => m.ReportID == intHoldIndex).ToList();
                db.JunctionReportLocations.RemoveRange(oldJRL);
                List<JunctionReportProgram> oldJRP = db.JunctionReportPrograms.Where(m => m.ReportID == intHoldIndex).ToList();
                db.JunctionReportPrograms.RemoveRange(oldJRP);
                List<JunctionReportRole> oldJRR = db.JunctionReportRoles.Where(m => m.ReportID == intHoldIndex).ToList();
                db.JunctionReportRoles.RemoveRange(oldJRR);
                List<JunctionReportType> oldJRT = db.JunctionReportTypes.Where(m => m.ReportID == intHoldIndex).ToList();
                db.JunctionReportTypes.RemoveRange(oldJRT);
                List<JunctionReportPopulation> oldJRPo = db.JunctionReportPopulations.Where(m => m.ReportID == intHoldIndex).ToList();
                db.JunctionReportPopulations.RemoveRange(oldJRPo);*/

                // Check to see if any of the sort checkboxes are checked and write to junction tables 
                if (rbvm.report.SortByDepartment)
                {
                    foreach (JunctionReportDepartment jrd in rbvm.junctionReportDepartments)
                    {
                        db.JunctionReportDepartments.Add(jrd);
                    }
                }
                if (rbvm.report.SortByLocation)
                {
                    foreach (JunctionReportLocation jrl in rbvm.junctionReportLocations)
                    {
                        db.JunctionReportLocations.Add(jrl);
                    }
                }
                if (rbvm.report.SortByProgram)
                {
                    foreach (JunctionReportProgram jrp in rbvm.junctionReportPrograms)
                    {
                        db.JunctionReportPrograms.Add(jrp);
                    }
                }
                if (rbvm.report.SortByPopulation)
                {
                    foreach (JunctionReportPopulation jrp in rbvm.junctionReportPopulations)
                    {
                        db.JunctionReportPopulations.Add(jrp);
                    }
                }
                if (rbvm.report.SortByRole)
                {
                    foreach (JunctionReportRole jrr in rbvm.junctionReportRoles)
                    {
                        db.JunctionReportRoles.Add(jrr);
                    }
                }
                if (rbvm.report.SortByType)
                {
                    foreach (JunctionReportType jrt in rbvm.junctionReportTypes)
                    {
                        db.JunctionReportTypes.Add(jrt);
                    }
                }
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                SetViewBag();
            }

            return View(rbvm);
        }

        // GET: /Report/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = await db.Reports.FindAsync(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: /Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Report report = await db.Reports.FindAsync(id);
            db.Reports.Remove(report);
            List<JunctionReportType> jrtList = await db.JunctionReportTypes.Where(m => m.ReportID == id).ToListAsync();
            db.JunctionReportTypes.RemoveRange(jrtList);
            List<JunctionReportRole> jrrList = await db.JunctionReportRoles.Where(m => m.ReportID == id).ToListAsync();
            db.JunctionReportRoles.RemoveRange(jrrList);
            List<JunctionReportProgram> jrprList = await db.JunctionReportPrograms.Where(m => m.ReportID == id).ToListAsync();
            db.JunctionReportPrograms.RemoveRange(jrprList);
            List<JunctionReportPopulation> jrpoList = await db.JunctionReportPopulations.Where(m => m.ReportID == id).ToListAsync();
            db.JunctionReportPopulations.RemoveRange(jrpoList);
            List<JunctionReportLocation> jrlList = await db.JunctionReportLocations.Where(m => m.ReportID == id).ToListAsync();
            db.JunctionReportLocations.RemoveRange(jrlList);
            List<JunctionReportDepartment> jrdList = await db.JunctionReportDepartments.Where(m => m.ReportID == id).ToListAsync();
            db.JunctionReportDepartments.RemoveRange(jrdList);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Ajax method for adding new location to the form
        public ActionResult CreateNewLocation()
        {
            ViewBag.LocID = new SelectList(db.Locations, "LocationID", "LocationName");
            return PartialView("~/Views/Shared/EditorTemplates/JunctionReportLocation.cshtml", new JunctionReportLocation());
        }

        // Ajax method for adding new program to the form
        public ActionResult CreateNewProgram()
        {
            ViewBag.ProID = new SelectList(db.Programs, "ProgramID", "ProgramName");
            return PartialView("~/Views/Shared/EditorTemplates/JunctionReportProgram.cshtml", new JunctionReportProgram());
        }

        // Ajax method for adding new department to the form
        public ActionResult CreateNewDepartment()
        {
            ViewBag.DepID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            return PartialView("~/Views/Shared/EditorTemplates/JunctionReportDepartment.cshtml", new JunctionReportDepartment());
        }

        // Ajax method for removing new department to the form
        public ActionResult CreateNewRole()
        {
            ViewBag.RolID = new SelectList(db.Roles, "RoleID", "RoleName");
            return PartialView("~/Views/Shared/EditorTemplates/JunctionReportRole.cshtml", new JunctionReportRole());
        }
        
        // Ajax method for removing new Population to the form
        public ActionResult CreateNewPopulation()
        {
            ViewBag.PopID = new SelectList(db.TargetPopulations, "PopulationID", "PopulationName");
            return PartialView("~/Views/Shared/EditorTemplates/JunctionReportPopulation.cshtml", new JunctionReportPopulation());
        }
        
        // Ajax method for removing new type to the form
        public ActionResult CreateNewType()
        {
            ViewBag.TypID = new SelectList(db.ActivityTypes, "TypeID", "TypeName");
            return PartialView("~/Views/Shared/EditorTemplates/JunctionReportType.cshtml", new JunctionReportType());
        }

        // Helper method to set ViewBag variables
        private void SetViewBag()
        {
            ViewBag.LocID = new SelectList(db.Locations, "LocationID", "LocationName");
            ViewBag.RolID = new SelectList(db.Roles, "RoleID", "RoleName");
            ViewBag.DepID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            ViewBag.ProID = new SelectList(db.Programs, "ProgramID", "ProgramName");
            ViewBag.PopID = new SelectList(db.TargetPopulations, "PopulationID", "PopulationName");
            ViewBag.TypID = new SelectList(db.ActivityTypes, "TypeID", "TypeName");
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
