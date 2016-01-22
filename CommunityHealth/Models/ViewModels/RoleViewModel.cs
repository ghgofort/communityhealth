using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityHealth.Models.ViewModels
{
    public class RoleViewModel
    {
        [Display(Name="NCHC Role")]
        public string RoleName { get; set; }
        public int RoleID { get; set; }
        public bool isSelected { get; set; }
    }
}