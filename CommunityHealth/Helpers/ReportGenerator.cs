using CommunityHealth.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityHealth.Helpers
{
    public static class ReportGenerator
    {
        public static ActivityReportModel getModel(ReportBuilderViewModel rbvm)
        {
            ActivityReportModel arm = new ActivityReportModel()
            {

            };
            return arm;
        }
    }
}