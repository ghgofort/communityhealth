using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityHealth.Models.ViewModels;
using CommunityHealth.Models;

namespace CommunityHealth.DAL
{
    /// <summary>
    /// This class provides a mechanism for data queries from within system operations that don't usually have 
    /// access to the data of the application, like our authorization data attribute.
    /// </summary>
    public class DataHandler
    {
        // Instance of the queryable Entity representation of the database
        private CommunityProgramsDBEntities db = new CommunityProgramsDBEntities();
        public DataHandler()
        {
            // Not currently implemented -- Could be used for setup tasks later
        }
        /// <summary>
        /// Returns the user name for the AddedBy field of the Event record for authentication purposes
        /// </summary>
        /// <param name="id">The ID for the Event that authentication is being performed on</param>
        /// <returns>the user name in string format -- This includes the "LAVA\" portion that represents the domain</returns>
        public string GetCreatedBy(int id)
        {
            string user = "";
            user = db.CommunityActions.First(m => m.ActionID == id).AddedBy;
            return user;
        }
    }
}