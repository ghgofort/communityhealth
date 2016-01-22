using System;
using System.ComponentModel.DataAnnotations;

namespace CommunityHealth.Models
{
    [MetadataType(typeof(ActivityTypeMetadata))]
    public partial class ActivityType{}

    [MetadataType(typeof(CommunityActionMetadata))]
    public partial class CommunityAction { }

    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department { }

    [MetadataType(typeof(LocationMetadata))]
    public partial class Location { }

    [MetadataType(typeof(ProgramMetadata))]
    public partial class Program { }

    [MetadataType(typeof(RoleMetadata))]
    public partial class Role { }

    [MetadataType(typeof(TargetPopulationMetadata))]
    public partial class TargetPopulation { }

    [MetadataType(typeof(JunctionDepartmentActionMetadata))]
    public partial class JunctionDepartmentAction { }

    [MetadataType(typeof(JunctionTypeActionMetadata))]
    public partial class JunctionTypeAction { }

    [MetadataType(typeof(ReportMetadata))]
    public partial class Report{ }
}