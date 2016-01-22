using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CommunityHealth.Models.ViewModels
{
    public class ActivityViewModel
    {
        public ActivityViewModel()
        {
            selectedPopulations = new List<int>();
        }
        [DisplayName("Activity Type")]
        public virtual IList<JunctionTypeAction> junctionTypeActions{ get; set; }
        [DisplayName("Target Populations")]
        public List<int> selectedPopulations { get; set; }
        [DisplayName("Department Involved")]
        public virtual IList<JunctionDepartmentAction> junctionDepartmentActions{ get; set; }
        public virtual CommunityAction communityAction { get; set; }
        [DisplayName("North Country's Role")]
        public virtual IList<RoleViewModel> roleViewModels { get; set; }
    }
}