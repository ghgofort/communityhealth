using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityHealth.Models.ViewModels
{
    public class PopulationViewModel
    {
        public int PopulationID { get; set; }
        public bool isSelected { get; set; }
        [Display(Name="Target Population")]
        public string PopulationName { get; set; }
    }
}