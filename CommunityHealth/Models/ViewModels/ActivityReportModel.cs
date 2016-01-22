using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityHealth.Models.ViewModels
{
    public class ActivityReportModel
    {
        [Display(Name="Descriptive Title")]
        public string DescriptiveTitle { get; set; }
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        [Display(Name = "Locations")]
        public string Location { get; set; }
        [Display(Name = "Programs")]
        public string Program { get; set; }
        [Display(Name = "Activity Types")]
        public string Type { get; set; }
        [Display(Name = "Participants Involved")]
        public string Participants { get; set; }
        [Display(Name = "Departments Involved")]
        public string Department { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "Target Populations")]
        public string Population { get; set; }
        [Display(Name = "NCHC Roles")]
        public string Role { get; set; }
        [Display(Name="Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Duration")]
        public string Duration { get; set; }
    }
}