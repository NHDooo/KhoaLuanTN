using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
namespace Models.Dao
{
   public  class GroupUserDao
    {
        MovieProjectDbcontext db = null;
        public GroupUserDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<UserGroup> ListPg(int page, int pageSize)
        {
            return db.UserGroups.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public UserGroup ViewDetail(long id)
        {
            return db.UserGroups.Find(id);
        }

        public bool Delete(string id)
        {
            try
            {
                var user = db.UserGroups.Find(id);
                db.UserGroups.Remove(user);
                db.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}