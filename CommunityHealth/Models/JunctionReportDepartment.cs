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
    
    public partial class JunctionReportDepartment
    {
        public int IndexID { get; set; }
        public int DepartmentID { get; set; }
        public int ReportID { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Report Report { get; set; }
    }
}
