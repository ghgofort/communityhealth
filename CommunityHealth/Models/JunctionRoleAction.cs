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
    
    public partial class JunctionRoleAction
    {
        public int IndexID { get; set; }
        public int RoleID { get; set; }
        public int ActionID { get; set; }
    
        public virtual CommunityAction CommunityAction { get; set; }
        public virtual Role Role { get; set; }
    }
}
