//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommunityHealth.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Role
    {
        public Role()
        {
            this.JunctionRoleActions = new HashSet<JunctionRoleAction>();
            this.JunctionReportRoles = new HashSet<JunctionReportRole>();
        }
    
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool isActive { get; set; }
        public byte[] RecordVersion { get; set; }
    
        public virtual ICollection<JunctionRoleAction> JunctionRoleActions { get; set; }
        public virtual ICollection<JunctionReportRole> JunctionReportRoles { get; set; }
    }
}
