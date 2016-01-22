using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityHealth.Models.ViewModels
{
    public class ReportBuilderViewModel
    {
        public ReportBuilderViewModel()
        {
            junctionReportLocations = new List<JunctionReportLocation>();
            junctionReportDepartments = new List<JunctionReportDepartment>();
            junctionReportPopulations = new List<JunctionReportPopulation>();
            junctionReportRoles = new List<JunctionReportRole>();
            junctionReportTypes = new List<JunctionReportType>();
            junctionReportPrograms = new List<JunctionReportProgram>();
        }
        [DataType(DataType.Date)]
        [Display(Name = "Beginning Date")]
        public System.DateTime? beginningReportDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public System.DateTime? endReportDate { get; set; }
        public virtual Report report {get;set;}
        [Display(Name="Location")]
        public virtual IList<JunctionReportLocation> junctionReportLocations { get; set; }
        [Display(Name = "Department")]
        public virtual IList<JunctionReportDepartment> junctionReportDepartments { get; set; }
        public virtual IList<JunctionReportPopulation> junctionReportPopulations { get; set; }
        [Display(Name = "NCHC Role")]
        public virtual IList<JunctionReportRole> junctionReportRoles { get; set; }
        [Display(Name = "Activity Type")]
        public virtual IList<JunctionReportType> junctionReportTypes { get; set; }
        [Display(Name = "Program")]
        public virtual IList<JunctionReportProgram> junctionReportPrograms { get; set; }
    }
}