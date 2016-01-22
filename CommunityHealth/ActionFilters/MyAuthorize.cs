using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using CommunityHealth.DAL;

namespace CommunityHealth.ActionFilters
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                return false;
            }

            var rd = httpContext.Request.RequestContext.RouteData;

            var id = rd.Values["id"];
            var userName = httpContext.User.Identity.Name;
            //Access the current record through the DataHandler class
            DataHandler dh = new DataHandler();
            string createdBy = dh.GetCreatedBy(int.Parse(id.ToString()));
            bool isAuthorized = false;
            if (userName.ToString() == createdBy) isAuthorized = true;
            var User = System.Web.HttpContext.Current.User;
            if (User.IsInRole(@"LAVA\CommunityEventAdmin"))isAuthorized=true;

            return isAuthorized;
        }
    }
}