using System.Web;
using System.Web.Mvc;
using CommunityHealth.DAL;

namespace CommunityHealth.ActionFilters
{
    /// <summary>
    /// This is a custom data attribute for authorizing against an Active Directory 
    /// security group. It is intended to be used as a mechanism for authorizing users
    /// to access sensative controller action methods.
    /// 
    /// It extends the Class System.Web.Mvc.AuthorizeAttribute
    /// </summary>
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Sets the security group to authorize against
        /// </summary>
        public string strSecurityGroup { get; set; }
        /// <summary>
        /// Overrides the AuthorizeCore method 
        /// </summary>
        /// <param name="httpContext">the context object of the request that needs to be authorized</param>
        /// <returns>boolean value for authorization results</returns>
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
            /*
             * Generalized version that accepts the security group as a parameter
             * Would be used as [MyAuthorize(strSecurityGroup="LAVA\CommunityEventAdmin")]
             * -----Hasn't been tested----
             * 
             * if(strSecurityGroup!=null)
             * {
             *   if (User.IsInRole(@strSecurityGroup)) isAuthorized = true;
             * }
             */ 
            if (User.IsInRole(@"LAVA\CommunityEventAdmin"))isAuthorized=true;

            return isAuthorized;
        }
    }
}