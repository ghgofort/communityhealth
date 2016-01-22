using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityHealth.Models.ViewModels;
using CommunityHealth.Models;

namespace CommunityHealth.DAL
{
    public class DataHandler
    {
        private CommunityProgramsDBEntities db = new CommunityProgramsDBEntities();
        public DataHandler()
        {

        }
        public IEnumerable<T> GetData<T>(IEnumerable<T> data)
        {
            return data;
        }
        public IEnumerable<ActivityViewModel> GetData(IEnumerable<ActivityViewModel> data, string search)
        {

            return data;
        }
        public string GetCreatedBy(int id)
        {
            string user = "";
            user = db.CommunityActions.First(m => m.ActionID == id).AddedBy;
            return user;
        }
    }
}