using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieProject.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { set; get; }
        public string UserName { set; get; }
        public string Name { set; get; }
        public int PermissonID { set; get; }
        public string GroupID { set; get; }
    }
}