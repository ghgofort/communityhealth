using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CommunityHealth.Models;
using CommunityHealth.Models.ViewModels;
using CommunityHealth.ActionFilters;
using System.Data.Entity.Core;
using PagedList;

namespace CommunityHealth.Controllers
{
    public class EventController : Controller
    {
        private CommunityProgramsDBEntities db = new CommunityProgramsDBEntities();

        // GET: /Event/Index
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchKey, int? page)
        {
            // Save the current sorting parameter
            ViewBag.CurrentSort = sortOrder;
            // Extract sort order from the query string
            ViewBag.DateSort =sortOrder=="date" ? "date_d" : "date";
            //ViewBag.DepSort = sortOrder == "dep" ? "dep_d" : "dep";
            ViewBag.AddedBySort = sortOrder == "add" ? "add_d" : "add";
            //ViewBag.ProgramSort = sortOrder == "prog" ? "prog_d" : "prog";

            if(searchKey!=null)
            {
                page = 1;
            }
            else
            {
                searchKey = currentFilter;
            }
            ViewBag.CurrentFilter = searchKey;

            
            //  ********************************************
            //  Create an instance of the Data Handler class
            //  Pass the search paramaters to the Data Handler
            //  
            //
            //  ********************************************
            var communityactions = await db.CommunityActions
                .Include(c => c.Location)
                .Include(c => c.Program).ToListAsync();
            List<ActivityViewModel> activityViewModels = new List<ActivityViewModel>();
            foreach(var action in communityactions)
            {
                activityViewModels.Add(new ActivityViewModel
                {
                    communityAction = action,
                    junctionTypeActions = await db.JunctionTypeActions.Where(d => d.ActionID == action.ActionID).ToListAsync(),
                    junctionDepartmentActions = await db.JunctionDepartmentActions.Where(d => d.ActionID == action.ActionID).ToListAsync()
                });
            }
            if(!String.IsNullOrEmpty(searchKey))
            {
                string sKey = searchKey.ToLower();
                activityViewModels = activityViewModels.Where(s => (s.communityAction.BreifDescription != null && s.communityAction.BreifDescription.ToLower().Contains(searchKey)) 
                    ||(s.communityAction.Notes !=null && s.communityAction.Notes.ToLower().Contains(searchKey))).ToList();
            }

            // Case statement to determin sorting order
            switch(sortOrder)
            {
                case "date_d":
                    activityViewModels = activityViewModels.OrderByDescending(s => s.communityAction.StartDate).ToList();
                    break;
                case "date":
                    activityViewModels = activityViewModels.OrderBy(s => s.communityAction.StartDate).ToList();
                    break;
                case "add":
                    activityViewModels = activityViewModels.OrderBy(s => s.communityAction.AddedBy).ToList();
                    break;
                case "add_d":
                    activityViewModels = activityViewModels.OrderByDescending(s => s.communityAction.AddedBy).ToList();
                    break;
                /*case "prog":
                    activityViewModels = activityViewModels.OrderBy(s => s.communityAction.Program.ProgramName != null ? s.communityAction.Program.ProgramName: string.Empty).ToList();
                    break;
                case "prog_d":
                    activityViewModels = activityViewModels.OrderByDescending(s => s.communityAction.Program.ProgramName !=null).ToList();
                    break;*/
                default:
                    activityViewModels = activityViewModels.OrderByDescending(s => s.communityAction.ActionID).ToList();
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var pageOfResults = activityViewModels.ToPagedList(pageNumber,pageSize);
            return View(pageOfResults);
        }

        // Action for adding a new Type
        public ActionResult CreateNewType()
        {
            ViewBag.TyID = new SelectList(db.ActivityTypes, "TypeID", "TypeName");
            return PartialView("~/Views/Shared/EditorTemplates/JunctionTypeAction.cshtml", new JunctionTypeAction());
        }

        // Action for adding a new department via Ajax call
        public ActionResult CreateNewDepartment()
        {
            ViewBag.DepID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            return PartialView("~/Views/Shared/EditorTemplates/JunctionDepartmentAction.cshtml", new JunctionDepartmentAction());
        }

        // GET: /Event/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityViewModel avModel = new ActivityViewModel();
            avModel.communityAction = await db.CommunityActions.FindAsync(id);
            avModel.junctionTypeActions = await db.JunctionTypeActions.Where(m => m.ActionID == avModel.communityAction.ActionID).ToListAsync();
            avModel.junctionDepartmentActions = await db.JunctionDepartmentActions.Where(m => m.ActionID == avModel.communityAction.ActionID).ToListAsync();
            if (avModel == null)
            {
                return HttpNotFound();
            }
            return View(avModel);
        }
        // GET: /Event/Create
        public ActionResult Create()
        {
            // Create lists of items for 
            SetViewBag();
            IList<RoleViewModel> roleViews = new List<RoleViewModel>();
            foreach(Role role in db.Roles)
            {
                roleViews.Add(new RoleViewModel()
                {
                    RoleName=role.RoleName,
                    RoleID = role.RoleID,
                    isSelected = false
                });
            }

            ActivityViewModel avModel = new ActivityViewModel
            {
                communityAction = new CommunityAction(),
                junctionTypeActions = new List<JunctionTypeAction>(),
                junctionDepartmentActions = new List<JunctionDepartmentAction>(),
                roleViewModels = roleViews
            };
            avModel.communityAction.AddedBy = User.Identity.Name;
            JunctionDepartmentAction jda = new JunctionDepartmentAction();
            JunctionTypeAction jta = new JunctionTypeAction();
            avModel.junctionDepartmentActions.Add(jda);
            avModel.junctionTypeActions.Add(jta);
            avModel.communityAction.StartDate = System.DateTime.Now;
            avModel.communityAction.EndDate = System.DateTime.Now;
            return View(avModel);
        }

        // POST: /Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ActivityViewModel newEvent)
        {
            if (ModelState.IsValid)
            {
                // Add the username to the AddedBy field
                newEvent.communityAction.AddedBy = User.Identity.Name;
                db.CommunityActions.Add(newEvent.communityAction);
                foreach(JunctionDepartmentAction jda in newEvent.junctionDepartmentActions)
                {
                    db.JunctionDepartmentActions.Add(jda);
                }
                foreach(JunctionTypeAction jta in newEvent.junctionTypeActions)
                {
                    db.JunctionTypeActions.Add(jta);
                }
                // Add the selected roles to the DBContext
                foreach(RoleViewModel rvm in newEvent.roleViewModels)
                {
                    if(rvm.isSelected)
                    {
                        db.JunctionRoleActions.Add(new JunctionRoleAction()
                        {
                            ActionID=newEvent.communityAction.ActionID,
                            RoleID=rvm.RoleID
                        });
                    }
                }
                // Add the selected populations to the DBContext if it isn't null

                foreach (int populationID in newEvent.selectedPopulations)
                {
                    db.JunctionPopulationActions.Add(new JunctionPopulationAction()
                    {
                        PopulationID = populationID,
                        ActionID = newEvent.communityAction.ActionID
                    });
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View(newEvent);
        }

        // GET: /Event/Edit/5
        [MyAuthorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Create the CommunityAction model to add to the view model.
            CommunityAction ca = await db.CommunityActions.FindAsync(id);
            if (ca == null)
            {
                return HttpNotFound();
            }
            ActivityViewModel activityviewmodel = new ActivityViewModel();
            activityviewmodel.communityAction = ca;
            // Get the Junction lists from the DBContext
            IList<JunctionRoleAction> junctionroleactions = await db.JunctionRoleActions.Where(d => d.ActionID == ca.ActionID).ToListAsync();
            IList<JunctionTypeAction> junctiontypeactions = await db.JunctionTypeActions.Where(d => d.ActionID == ca.ActionID).ToListAsync();
            IList<JunctionDepartmentAction> junctiondepartmentactions = await db.JunctionDepartmentActions.Where(d => d.ActionID == ca.ActionID).ToListAsync();
            IList<JunctionPopulationAction> junctionpopulationactions = await db.JunctionPopulationActions.Where(d => d.ActionID==ca.ActionID).ToListAsync();            
            // Create list of Role 
            IList<RoleViewModel> roleViews = new List<RoleViewModel>();
            // Add all the roles to the list and select the proper roles
            foreach (Role role in db.Roles)
            {
                //Create each RoleViewModel to display
                RoleViewModel availableRole = new RoleViewModel();
                availableRole.RoleName = role.RoleName;
                availableRole.RoleID = role.RoleID;
                availableRole.isSelected = false;
                foreach(JunctionRoleAction r in junctionroleactions)
                {
                    if (role.RoleID == r.RoleID)availableRole.isSelected = true;

                }
                roleViews.Add(availableRole);
            }
            // Create a list of the selected populations
            List<int> intSelected = new List<int>();
            foreach(JunctionPopulationAction jpa in junctionpopulationactions)
            {
                intSelected.Add(jpa.PopulationID);
            }
            // Add the lists to the ActivityViewModel properties 
            activityviewmodel.selectedPopulations = intSelected;
            activityviewmodel.roleViewModels = roleViews;
            activityviewmodel.junctionTypeActions = junctiontypeactions;
            activityviewmodel.junctionDepartmentActions = junctiondepartmentactions;
            SetViewBag();
            return View(activityviewmodel);
        }
        
        // POST: /Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ActivityViewModel vmActivity)
        {
            if (ModelState.IsValid)
            {

                // Get the unmodified version to compare to the modified version.
                CommunityAction ca = await db.CommunityActions.FindAsync(vmActivity.communityAction.ActionID);
                
                //**************Delete the entries that have been removed from the model****************
                // Create a list of JunctionDepartmentActions in our ViewModel for enumeration
                List<JunctionDepartmentAction> jdas = new List<JunctionDepartmentAction>(vmActivity.junctionDepartmentActions);
                List<JunctionTypeAction> jtas = new List<JunctionTypeAction>(vmActivity.junctionTypeActions);
                // Delete any records from junction table for departments involved in the activity
                foreach(JunctionDepartmentAction jd in ca.JunctionDepartmentActions)
                {
                    // If the items are in both the database and the ViewModel 
                    // Set the ViewState to Modified and then delete them from the 
                    // ViewModel because we don't need to add them again
                    if (!(vmActivity.junctionDepartmentActions.Any(d => d.DepartmentID == jd.DepartmentID)))
                    {
                        jdas.Add(jd);
                    }
                }
                // Delete any records that are in the junction table but not in the View Model
                foreach (JunctionTypeAction jt in ca.JunctionTypeActions)
                {
                    // If the items are in both the database and the ViewModel then delete them from the ViewModel because we don't need to add them again
                    if (!(vmActivity.junctionTypeActions.Any(d => d.TypeID == jt.TypeID)))
                    {
                        // Add the item to a list to be removed after iteration is complete
                        jtas.Add(jt);
                    }
                }
                // Remove the list of types that are no longer selected for the activity from the database
                foreach(JunctionTypeAction jt in jtas)
                {
                    db.JunctionTypeActions.RemoveRange(db.JunctionTypeActions.Where(d=>d.IndexID ==jt.IndexID));
                }
                // Remove the list of departments that are no longer selected for the activity from the database
                foreach (JunctionDepartmentAction jd in jdas)
                {
                    db.JunctionDepartmentActions.RemoveRange(db.JunctionDepartmentActions.Where(d => d.IndexID == jd.IndexID));
                }

                //**************Add any remaining junction items in the ViewModel to the Database****************
                // Add the Junction Table entries for Departments involved in Activity
                foreach(JunctionDepartmentAction jd in vmActivity.junctionDepartmentActions)
                {
                    // Add the Action ID to any new Department junctions
                    jd.ActionID = vmActivity.communityAction.ActionID;
                    db.JunctionDepartmentActions.Add(jd);
                }
                // Add the Junction Table entries for ActivityType for each Activity if they don't already exist in the DB
                foreach (JunctionTypeAction jt in vmActivity.junctionTypeActions)
                {
                    jt.ActionID = vmActivity.communityAction.ActionID;
                    db.JunctionTypeActions.Add(jt);                    
                }
                // Remove all of the JunctionRoleActions for this action
                db.JunctionRoleActions.RemoveRange(new List<JunctionRoleAction>(await db.JunctionRoleActions.Where(d => d.ActionID == vmActivity.communityAction.ActionID).ToListAsync()));
                // Check for changes to the roles and modify DBContext with changes
                foreach (RoleViewModel rvm in vmActivity.roleViewModels)
                {
                    if (rvm.isSelected)
                    {
                        db.JunctionRoleActions.Add(new JunctionRoleAction
                        {
                            RoleID=rvm.RoleID,
                            ActionID=vmActivity.communityAction.ActionID
                        });
                    }
                }
                // Remove old JunctionPopulationAction items from DBContext
                db.JunctionPopulationActions.RemoveRange(new List<JunctionPopulationAction>(await db.JunctionPopulationActions.Where(d=>d.ActionID == vmActivity.communityAction.ActionID).ToListAsync()));
                IList<int> intIDs = new List<int>();
                //Add the selected JunctionPopulationActions 
                foreach(int a in vmActivity.selectedPopulations)
                {
                    db.JunctionPopulationActions.Add(new JunctionPopulationAction
                    {
                        PopulationID=a,
                        ActionID = vmActivity.communityAction.ActionID
                    });
                }

                //***************** Make changes to the CommunityAction record ****************  
                // Save the CommunityAction Record back to the db
                db.Entry(ca).CurrentValues.SetValues(vmActivity.communityAction);

                // Save the changes to the DB
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                catch(OptimisticConcurrencyException oce)
                {
                    oce.ToString();
                }
                catch(DBConcurrencyException dbce)
                {
                    dbce.ToString();
                }
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View(vmActivity);
        }

        // GET: /Event/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityViewModel activityViewModel = new ActivityViewModel();
            CommunityAction ca = await db.CommunityActions.FindAsync(id);
            if (ca == null)
            {
                return HttpNotFound();
            }
            // Get the Junction lists from the DBContext
            IList<JunctionRoleAction> junctionroleactions = await db.JunctionRoleActions.Where(d => d.ActionID == ca.ActionID).ToListAsync();
            IList<JunctionTypeAction> junctiontypeactions = await db.JunctionTypeActions.Where(d => d.ActionID == ca.ActionID).ToListAsync();
            IList<JunctionDepartmentAction> junctiondepartmentactions = await db.JunctionDepartmentActions.Where(d => d.ActionID == ca.ActionID).ToListAsync();
            IList<JunctionPopulationAction> junctionpopulationactions = await db.JunctionPopulationActions.Where(d => d.ActionID == ca.ActionID).ToListAsync(); 
            activityViewModel.communityAction = ca;
            activityViewModel.junctionDepartmentActions = junctiondepartmentactions;
            activityViewModel.junctionTypeActions = junctiontypeactions;
            return View(activityViewModel);
        }

        // POST: /Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CommunityAction communityaction = await db.CommunityActions.FindAsync(id);
            IList<JunctionTypeAction> typeJunctions = await db.JunctionTypeActions.Where(m => m.ActionID == id).ToListAsync();
            foreach(JunctionTypeAction jta in typeJunctions)
            {
                db.JunctionTypeActions.Remove(jta);
            }
            IList<JunctionDepartmentAction> depJunctions = await db.JunctionDepartmentActions.Where(m => m.ActionID == id).ToListAsync();
            foreach (JunctionDepartmentAction jda in depJunctions)
            {
                db.JunctionDepartmentActions.Remove(jda);
            }
            IList<JunctionPopulationAction> populationJunctions = await db.JunctionPopulationActions.Where(m => m.ActionID == id).ToListAsync();
            foreach (JunctionPopulationAction jpa in populationJunctions)
            {
                db.JunctionPopulationActions.Remove(jpa);
            }
            IList<JunctionRoleAction> roleJunctions = await db.JunctionRoleActions.Where(m => m.ActionID == id).ToListAsync();
            foreach (JunctionRoleAction jra in roleJunctions)
            {
                db.JunctionRoleActions .Remove(jra);
            }
            db.CommunityActions.Remove(communityaction);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Helper method to set Viewbag lists
        private void SetViewBag()
        {
            ViewBag.RoleID = new SelectList(db.Roles.OrderBy(m => m.RoleName), "RoleID", "RoleName");
            ViewBag.DepID = new SelectList(db.Departments.OrderBy(m => m.DepartmentName) , "DepartmentID", "DepartmentName");
            ViewBag.TyID = new SelectList(db.ActivityTypes.OrderBy(m => m.TypeName) , "TypeID", "TypeName");
            ViewBag.PopID = new SelectList(db.TargetPopulations.OrderBy(m => m.PopulationName), "PopulationID", "PopulationName");
            ViewBag.LocID = new SelectList(db.Locations.OrderBy(m => m.LocationName), "LocationID", "LocationName");
            ViewBag.ProID = new SelectList(db.Programs.OrderBy(m => m.ProgramName), "ProgramID", "ProgramName");
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
