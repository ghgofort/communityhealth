using System;
using System.ComponentModel.DataAnnotations;
namespace CommunityHealth.Models
{
    public class ActivityTypeMetadata
    {
        [Display(Name = "Activity Type")]
        [Required]
        public string TypeName;
        [Display(Name="Active")]
        [Required]
        public bool isActive;
    }
    public class CommunityActionMetadata
    {
        [DataType(DataType.Date)]
        [Display(Name="Start Date")]
        [Required]
        public System.DateTime StartDate;
        [DataType(DataType.Date)]
        [Display(Name="End Date")]
        public Nullable<System.DateTime> EndDate;
        [Display(Name="Descriptive Title")]
        [Required]
        public string BreifDescription;
        [Display(Name="Duration (in hours)")]
        public Nullable<double> Duration;
        [Display(Name = "Other Location")]
        public string SubLocation;
        [Display(Name = "Added by")]
        public string AddedBy;
    }
    public class DepartmentMetadata
    {
        [Display(Name="Depatment")]
        [Required]
        public string DepartmentName;
        [Display(Name="Active")]
        [Required]
        public bool isActive;
    }
    public class LocationMetadata
    {
        [Display(Name="Location")]
        [Required]
        public string LocationName;
        [Display(Name="Active")]
        [Required]
        public bool isActive;
    }
    public class ProgramMetadata
    {
        [Display(Name="Program")]
        [Required]
        public string ProgramName;
        [Display(Name = "Active")]
        [Required]
        public bool isActive;
    }
    public class RoleMetadata
    {
        [Display(Name="North Country's Role")]
        [Required]
        public string RoleName;
        [Display(Name="Active")]
        [Required]
        public bool isActive;
    }
    public class TargetPopulationMetadata
    {
        [Display(Name = "Target Population")]
        [Required]
        public string PopulationName;
        [Display(Name="Active")]
        [Required]
        public bool isActive;
    }
    public class JunctionDepartmentActionMetadata
    {
        [Display(Name="Other Department")]
        public string SubDepartment;
    }
    public class JunctionTypeActionMetadata
    {
        [Display(Name="Number of Participants")]
        [Required]
        public int Participants;
    }
    public class ReportMetadata
    {
        [Display(Name="Search...")]
        public string SearchValue;
        [Required]
        [Display(Name="Search Description Field")]
        public bool SearchBreifDescription;
        [Required]
        [Display(Name = "Sort by Duration")]
        public bool SortByDuration;
        [DataType(DataType.Date)]
        [Display(Name = "Beginning Date")]
        public System.DateTime BeginReportDate;
        [DataType(DataType.Date)]
        [Display(Name = "Ending Date")]
        public System.DateTime EndReportDate;
        [Required]
        [Display(Name= "Sort by Date")]
        public bool SortByDate;
        [Display(Name = "Minimum Duration (in hours)")]
        public Nullable<double> LowDuration;
        [Display(Name = "Maximum Duration (in hours)")]
        public Nullable<double> HighDuration;
        [Required]
        [Display(Name = "Sort by Location")]
        public bool SortByLocation;
        [Required]
        [Display(Name = "Sort by Program")]
        public bool SortByProgram;
        [Required]
        [Display(Name = "Sort by Department")]
        public bool SortByDepartment;
        [Required]
        [Display(Name = "Sort by Population")]
        public bool SortByPopulation;
        [Required]
        [Display(Name = "Sort by NCHC Role")]
        public bool SortByRole;
        [Required]
        [Display(Name = "Sort by Activity Type")]
        public bool SortByType;
        [Required]
        [Display(Name = "Search Department Field")]
        public bool SearchDepartment;
        [Required]
        [Display(Name = "Search Notes Field")]
        public bool SearchNotes;
        [Required]
        [Display(Name = "Sort by who added record")]
        public bool SortAddedBy;
        [Display(Name = "Report Name")]
        public string ReportName { get; set; }
    }
}